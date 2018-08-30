using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Nyantilities.Controls
{
    public class NyaButton : Button
    {
        #region Dependency Properties

        public KeyboardAccelerator KeyboardAccelerator
        {
            get { return (KeyboardAccelerator)GetValue(KeyboardAcceleratorProperty); }
            set { SetValue(KeyboardAcceleratorProperty, value); }
        }

        public static readonly DependencyProperty KeyboardAcceleratorProperty =
            DependencyProperty.Register("KeyboardAccelerator", typeof(KeyboardAccelerator), typeof(NyaButton), new PropertyMetadata(null, OnKeyboardAcceleratorChanged));
        
        #endregion
        
        #region Properties Changed
        
        private static void OnKeyboardAcceleratorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NyaButton nyaButton = d as NyaButton;

            foreach (KeyboardAccelerator accelerator in nyaButton.KeyboardAccelerators)
            {
                accelerator.Invoked -= nyaButton.KeyboardAccelerator_Invoked;
            }

            nyaButton.KeyboardAccelerators.Clear();
            
            if(e.NewValue != null)
            {
                KeyboardAccelerator accelerator = new KeyboardAccelerator()
                {
                      Key        = nyaButton.KeyboardAccelerator.Key
                    , IsEnabled  = nyaButton.KeyboardAccelerator.IsEnabled
                    , Modifiers  = nyaButton.KeyboardAccelerator.Modifiers
                    , ScopeOwner = nyaButton.KeyboardAccelerator.ScopeOwner
                };

                accelerator.Invoked += nyaButton.KeyboardAccelerator_Invoked;

                nyaButton.KeyboardAccelerators.Add(accelerator);
            }
        }

        #endregion

        #region Keyboard Events

        private void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Command != null && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }

            args.Handled = true;
        }

        #endregion
    }
}
