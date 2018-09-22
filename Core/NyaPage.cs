using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Nyantilities.Core
{
    public class NyaPage : Page
    {
        #region Properties

        public NyaApp Application => NyaApp.Current;

        private NyaViewModel viewModel;
        public NyaViewModel ViewModel
        {
            get => viewModel;
            set
            {
                if(ViewModel != value)
                {
                    viewModel = value;
                    DataContext = value;
                }
            }
        }

        #endregion

        #region Constructor

        public NyaPage()
        {
            navigationHelper = new NyavigationHelper(this);
            navigationHelper.SaveState += NavigationHelper_SaveState;
            navigationHelper.LoadState += NavigationHelper_LoadState;
        }

        #endregion

        #region Page Navigation

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;

            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;

            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region NavigationHelper

        private NyavigationHelper navigationHelper;
        public NyavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
            set { navigationHelper = value; }
        }

        protected virtual void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            NyaViewModel vm = DataContext as NyaViewModel;

            if (vm != null)
            {
                vm.LoadState(e);
            }
            else
            {
                DebugHelper.WriteLine<NyaViewModel>("No ViewModel attached");
            }
        }

        protected virtual void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            NyaViewModel vm = DataContext as NyaViewModel;

            if (vm != null)
            {
                vm.SaveState(e);
            }
            else
            {
                DebugHelper.WriteLine<NyaViewModel>("No ViewModel attached");
            }
        }

        #endregion

        #region Util

        //muss rein, sonst spinnt die app komplett rum weil wenn ich alt drücke die app, warum auch immer, abstürzt ... >.>
        //sachen wie alt+num3 für ♥ kann ich aber nachwievor nicht machen weil die app dann troztdem abstürzt -.- 
        protected virtual void Dispatcher_AcceleratorKeyActivated(Windows.UI.Core.CoreDispatcher sender, Windows.UI.Core.AcceleratorKeyEventArgs args)
        {
            args.Handled = args.VirtualKey == VirtualKey.Menu;
        }

        public T LoadValue<T>(LoadStateEventArgs args, String key)
        {
            if (args.PageState?.ContainsKey(key) == true)
            {
                return (T)args.PageState[key];
            }
            return default(T);
        }

        public void SaveValue(SaveStateEventArgs args, String key, object value)
        {
            args.PageState[key] = value;
        }

        public static void SetBinding(FrameworkElement control, DependencyProperty property, object source, String path = null)
        {
            Binding binding = new Binding()
            {
                Source = source,
                Path = new PropertyPath(path)
            };
            control.SetBinding(property, binding);
        }

        public static KeyboardAccelerator CreateKeyboardAccelerator(VirtualKey key, DependencyObject scopeOwner = null, bool isEnabled = true, VirtualKeyModifiers modifiers = VirtualKeyModifiers.None)
        {
            return new KeyboardAccelerator()
            {
                  IsEnabled  = isEnabled
                , Key        = key
                , Modifiers  = modifiers
                , ScopeOwner = scopeOwner
            };
        }

        #endregion
    }
}
