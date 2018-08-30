using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Nyantilities.Core
{
    public abstract class NyaApp : Application
    {
        #region Properties

        //public event SuspendingEventHandler AppSuspending;
        //public event EventHandler<object> AppResuming;

        public static new NyaApp Current
        {
            get => Application.Current as NyaApp;
        }

        private bool isInBackgroundMode;
        public bool IsInBackgroundMode
        {
            get { return isInBackgroundMode; }
            set { isInBackgroundMode = value; }
        }

        private bool appIsSuspending = false;
        public bool IsSuspending
        {
            get { return appIsSuspending; }
            set { appIsSuspending = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
        /// und daher das logische Äquivalent von main() bzw. WinMain().
        /// </summary>
        public NyaApp()
        {
            Suspending += NyaApp_Suspending;
            Resuming   += NyaApp_Resuming;

            EnteredBackground += NyaApp_EnteredBackground;
            LeavingBackground += NyaApp_LeavingBackground;

            MemoryManager.AppMemoryUsageLimitChanging += MemoryManager_AppMemoryUsageLimitChanging;
            MemoryManager.AppMemoryUsageIncreased     += MemoryManager_AppMemoryUsageIncreased;
        }

        #endregion

        #region App Suspending

        /// <summary>
        /// Wird aufgerufen, wenn die Ausführung der Anwendung angehalten wird.  Der Anwendungszustand wird gespeichert,
        /// ohne zu wissen, ob die Anwendung beendet oder fortgesetzt wird und die Speicherinhalte dabei
        /// unbeschädigt bleiben.
        /// </summary>
        /// <param name="sender">Die Quelle der Anhalteanforderung.</param>
        /// <param name="e">Details zur Anhalteanforderung.</param>
        private async void NyaApp_Suspending(object sender, SuspendingEventArgs e)
        {
            DebugHelper.WriteLine<NyaApp>("Save AppState");

            //TODO: Anwendungszustand speichern und alle Hintergrundaktivitäten beenden
            var deferral = e.SuspendingOperation.GetDeferral();
            
            IsSuspending = true;

            await SuspensionManager.SaveAsync();

            OnSuspending(sender, e);

            DebugHelper.WriteLine<NyaApp>("Saving AppState Complete");
            deferral.Complete();
        }

        protected virtual void OnSuspending(object sender, SuspendingEventArgs args)
        {

        }

        #endregion

        #region App Resuming

        protected void NyaApp_Resuming(object sender, object e)
        {
            DebugHelper.WriteLine<NyaApp>("Restoring AppState");
            
            OnResuming(sender, e);

            DebugHelper.WriteLine<NyaApp>("Restoring AppState Complete");
        }

        protected virtual void OnResuming(object sender, object args)
        {

        }

        #endregion

        #region Memory Management

        /// <summary>
        /// Raised when the memory limit for the app is changing, such as when the app
        /// enters the background.
        /// </summary>
        /// <remarks>
        /// If the app is using more than the new limit, it must reduce memory within 2 seconds
        /// on some platforms in order to avoid being suspended or terminated.
        /// 
        /// While some platforms will allow the application
        /// to continue running over the limit, reducing usage in the time
        /// allotted will enable the best experience across the broadest range of devices.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void MemoryManager_AppMemoryUsageLimitChanging(object sender, AppMemoryUsageLimitChangingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Memory usage limit changing from "
                + (e.OldLimit / 1024) + "K to "
                + (e.NewLimit / 1024) + "K");

            // If app memory usage is over the limit about to be enforced,
            // then reduce usage to avoid suspending.
            if (MemoryManager.AppMemoryUsage >= e.NewLimit)
            {
                ReduceMemoryUsage(e.NewLimit);
            }
        }

        /// <summary>
        /// Handle system notifications that the app has increased its
        /// memory usage level compared to its current target.
        /// </summary>
        /// <remarks>
        /// The app may have increased its usage or the app may have moved
        /// to the background and the system lowered the target the app
        /// is expected to reach. Either way, if the application wants
        /// to maintain its priority to avoid being suspended before
        /// other apps, it may need to reduce its memory usage.
        /// 
        /// This is not a replacement for handling AppMemoryUsageLimitChanging
        /// which is critical to ensure the app immediately gets below the new
        /// limit. However, once the app is allowed to continue running and
        /// policy is applied, some apps may wish to continue monitoring
        /// usage to ensure they remain below the limit.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void MemoryManager_AppMemoryUsageIncreased(object sender, object e)
        {
            System.Diagnostics.Debug.WriteLine("Memory usage increased");

            // Obtain the current usage level
            var level = MemoryManager.AppMemoryUsageLevel;

            // Check the usage level to determine whether reducing memory is necessary.
            // Memory usage may have been fine when initially entering the background but
            // a short time later the app might be using more memory and need to trim back.
            if (level == AppMemoryUsageLevel.OverLimit || level == AppMemoryUsageLevel.High)
            {
                ReduceMemoryUsage(MemoryManager.AppMemoryUsageLimit);
            }
        }

        /// <summary>
        /// Reduces application memory usage.
        /// </summary>
        /// <remarks>
        /// When the app enters the background, receives a memory limit changing
        /// event, or receives a memory usage increased
        /// event it can optionally unload cached data or even its view content in 
        /// order to reduce memory usage and the chance of being suspended.
        /// 
        /// This must be called from multiple event handlers because an application may already
        /// be in a high memory usage state when entering the background or it
        /// may be in a low memory usage state with no need to unload resources yet
        /// and only enter a higher state later.
        /// </remarks>
        protected virtual void ReduceMemoryUsage(ulong limit)
        {
            // If the app has caches or other memory it can free, now is the time.
            // << App can release memory here >>

            // Additionally, if the application is currently
            // in background mode and still has a view with content
            // then the view can be released to save memory and 
            // can be recreated again later when leaving the background.
            if (isInBackgroundMode && Window.Current.Content != null)
            {
                System.Diagnostics.Debug.WriteLine("Unloading view");

                // Clear the view content. Note that views should rely on
                // events like Page.Unloaded to further release resources. Be careful
                // to also release event handlers in views since references can
                // prevent objects from being collected. C++ developers should take
                // special care to use weak references for event handlers where appropriate.
                Window.Current.Content = null;

                // Finally, clearing the content above and calling GC.Collect() below 
                // is what will trigger each Page.Unloaded handler to be called.
                // In order for the resources each page has allocated to be released,
                // it is necessary that each Page also call GC.Collect() from its
                // Page.Unloaded handler.
            }

            // Run the GC to collect released resources, including triggering
            // each Page.Unloaded handler to run.
            GC.Collect();

            System.Diagnostics.Debug.WriteLine("Finished reducing memory usage");
        }

        #endregion

        #region Enter/Leave Background

        /// <summary>
        /// Is called every time the app leaves the background
        /// </summary>
        protected virtual void NyaApp_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {

        }

        /// <summary>
        /// Is called every time the app enters the background
        /// </summary>
        protected virtual void NyaApp_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {

        }

        #endregion

        #region On Launched

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // App-Initialisierung nicht wiederholen, wenn das Fenster bereits Inhalte enthält.
            // Nur sicherstellen, dass das Fenster aktiv ist.
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Frame erstellen, der als Navigationskontext fungiert und zum Parameter der ersten Seite navigieren
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Zustand von zuvor angehaltener Anwendung laden
                }

                // Den Frame im aktuellen Fenster platzieren
                Window.Current.Content = rootFrame;
            }

            if (args.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Wenn der Navigationsstapel nicht wiederhergestellt wird, zur ersten Seite navigieren
                    // und die neue Seite konfigurieren, indem die erforderlichen Informationen als Navigationsparameter
                    // übergeben werden
                    OnNavigateTo(rootFrame, args);
                }
                // Sicherstellen, dass das aktuelle Fenster aktiv ist
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// is called when the app launches and should be used to navigate to the first Page in the application
        /// </summary>
        /// <param name="rootFrame"></param>
        /// <param name="args"></param>
        protected abstract void OnNavigateTo(Frame rootFrame, LaunchActivatedEventArgs args);

        /// <summary>
        /// Wird aufgerufen, wenn die Navigation auf eine bestimmte Seite fehlschlägt
        /// </summary>
        /// <param name="sender">Der Rahmen, bei dem die Navigation fehlgeschlagen ist</param>
        /// <param name="e">Details über den Navigationsfehler</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new System.Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        #endregion
    }
}
