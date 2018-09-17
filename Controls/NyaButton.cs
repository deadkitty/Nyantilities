using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Nyantilities.Controls
{
    public class NyaButton : Button
    {
        #region Dependency Properties
        
        public String ShortcutKey
        {
            get { return (String)GetValue(ShortcutKeyProperty); }
            set { SetValue(ShortcutKeyProperty, value); }
        }

        public static readonly DependencyProperty ShortcutKeyProperty =
            DependencyProperty.Register("ShortcutKey", typeof(String), typeof(NyaButton), new PropertyMetadata(null, ShortcutKeyChanged));

        private static void ShortcutKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            NyaButton nyaButton = d as NyaButton;

            foreach (KeyboardAccelerator accelerator in nyaButton.KeyboardAccelerators)
            {
                accelerator.Invoked -= nyaButton.KeyboardAccelerator_Invoked;
            }

            nyaButton.KeyboardAccelerators.Clear();

            if (args.NewValue != null)
            {
                nyaButton.KeyboardAccelerators.Add(nyaButton.CreateKey(args.NewValue as String));
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

        #region Create Key

        private KeyboardAccelerator CreateKey(String key)
        {
            String[] keyParts = key.Split('+');

            VirtualKeyModifiers modifier = VirtualKeyModifiers.None;

            if (keyParts.Length > 1)
            {
                switch (keyParts[0].ToLower())
                {
                    case "ctrl" :
                    case "strg" : modifier = VirtualKeyModifiers.Control; break;
                    case "shift": modifier = VirtualKeyModifiers.Shift; break;
                    case "win"  : modifier = VirtualKeyModifiers.Windows; break;
                    case "menu" : modifier = VirtualKeyModifiers.Menu; break;
                }
            }

            KeyboardAccelerator accelerator = new KeyboardAccelerator()
            {
                  Key = GetKey(keyParts.Last())
                , IsEnabled = IsEnabled
                , Modifiers = modifier
                , ScopeOwner = this
            };

            accelerator.Invoked += KeyboardAccelerator_Invoked;

            return accelerator;
        }
        
        private static VirtualKey GetKey(String key)
        {
            switch (key)
            {
                case "None"                        : return VirtualKey.None;
                case "LeftButton"                  : return VirtualKey.LeftButton;
                case "RightButton"                 : return VirtualKey.RightButton;
                case "Cancel"                      : return VirtualKey.Cancel;
                case "MiddleButton"                : return VirtualKey.MiddleButton;
                case "XButton1"                    : return VirtualKey.XButton1;
                case "XButton2"                    : return VirtualKey.XButton2;
                case "Back"                        : return VirtualKey.Back;
                case "Tab"                         : return VirtualKey.Tab;
                case "Clear"                       : return VirtualKey.Clear;
                case "Enter"                       : return VirtualKey.Enter;
                case "Shift"                       : return VirtualKey.Shift;
                case "Control"                     : return VirtualKey.Control;
                case "Menu"                        : return VirtualKey.Menu;
                case "Pause"                       : return VirtualKey.Pause;
                case "CapitalLock"                 : return VirtualKey.CapitalLock;
                case "Kana"                        : return VirtualKey.Kana;
                case "Hangul"                      : return VirtualKey.Hangul;
                case "Junja"                       : return VirtualKey.Junja;
                case "Final"                       : return VirtualKey.Final;
                case "Hanja"                       : return VirtualKey.Hanja;
                case "Kanji"                       : return VirtualKey.Kanji;
                case "Escape"                      : return VirtualKey.Escape;
                case "Convert"                     : return VirtualKey.Convert;
                case "NonConvert"                  : return VirtualKey.NonConvert;
                case "Accept"                      : return VirtualKey.Accept;
                case "ModeChange"                  : return VirtualKey.ModeChange;
                case "Space"                       : return VirtualKey.Space;
                case "PageUp"                      : return VirtualKey.PageUp;
                case "PageDown"                    : return VirtualKey.PageDown;
                case "End"                         : return VirtualKey.End;
                case "Home"                        : return VirtualKey.Home;
                case "Left"                        : return VirtualKey.Left;
                case "Up"                          : return VirtualKey.Up;
                case "Right"                       : return VirtualKey.Right;
                case "Down"                        : return VirtualKey.Down;
                case "Select"                      : return VirtualKey.Select;
                case "Print"                       : return VirtualKey.Print;
                case "Execute"                     : return VirtualKey.Execute;
                case "Snapshot"                    : return VirtualKey.Snapshot;
                case "Insert"                      : return VirtualKey.Insert;
                case "Delete"                      : return VirtualKey.Delete;
                case "Help"                        : return VirtualKey.Help;
                case "0"                           : return VirtualKey.Number0;
                case "1"                           : return VirtualKey.Number1;
                case "2"                           : return VirtualKey.Number2;
                case "3"                           : return VirtualKey.Number3;
                case "4"                           : return VirtualKey.Number4;
                case "5"                           : return VirtualKey.Number5;
                case "6"                           : return VirtualKey.Number6;
                case "7"                           : return VirtualKey.Number7;
                case "8"                           : return VirtualKey.Number8;
                case "9"                           : return VirtualKey.Number9;
                case "A"                           : return VirtualKey.A;
                case "B"                           : return VirtualKey.B;
                case "C"                           : return VirtualKey.C;
                case "D"                           : return VirtualKey.D;
                case "E"                           : return VirtualKey.E;
                case "F"                           : return VirtualKey.F;
                case "G"                           : return VirtualKey.G;
                case "H"                           : return VirtualKey.H;
                case "I"                           : return VirtualKey.I;
                case "J"                           : return VirtualKey.J;
                case "K"                           : return VirtualKey.K;
                case "L"                           : return VirtualKey.L;
                case "M"                           : return VirtualKey.M;
                case "N"                           : return VirtualKey.N;
                case "O"                           : return VirtualKey.O;
                case "P"                           : return VirtualKey.P;
                case "Q"                           : return VirtualKey.Q;
                case "R"                           : return VirtualKey.R;
                case "S"                           : return VirtualKey.S;
                case "T"                           : return VirtualKey.T;
                case "U"                           : return VirtualKey.U;
                case "V"                           : return VirtualKey.V;
                case "W"                           : return VirtualKey.W;
                case "X"                           : return VirtualKey.X;
                case "Y"                           : return VirtualKey.Y;
                case "Z"                           : return VirtualKey.Z;
                case "LeftWindows"                 : return VirtualKey.LeftWindows;
                case "RightWindows"                : return VirtualKey.RightWindows;
                case "Application"                 : return VirtualKey.Application;
                case "Sleep"                       : return VirtualKey.Sleep;
                case "NumberPad0"                  : return VirtualKey.NumberPad0;
                case "NumberPad1"                  : return VirtualKey.NumberPad1;
                case "NumberPad2"                  : return VirtualKey.NumberPad2;
                case "NumberPad3"                  : return VirtualKey.NumberPad3;
                case "NumberPad4"                  : return VirtualKey.NumberPad4;
                case "NumberPad5"                  : return VirtualKey.NumberPad5;
                case "NumberPad6"                  : return VirtualKey.NumberPad6;
                case "NumberPad7"                  : return VirtualKey.NumberPad7;
                case "NumberPad8"                  : return VirtualKey.NumberPad8;
                case "NumberPad9"                  : return VirtualKey.NumberPad9;
                case "Multiply"                    : return VirtualKey.Multiply;
                case "Add"                         : return VirtualKey.Add;
                case "Separator"                   : return VirtualKey.Separator;
                case "Subtract"                    : return VirtualKey.Subtract;
                case "Decimal"                     : return VirtualKey.Decimal;
                case "Divide"                      : return VirtualKey.Divide;
                case "F1"                          : return VirtualKey.F1;
                case "F2"                          : return VirtualKey.F2;
                case "F3"                          : return VirtualKey.F3;
                case "F4"                          : return VirtualKey.F4;
                case "F5"                          : return VirtualKey.F5;
                case "F6"                          : return VirtualKey.F6;
                case "F7"                          : return VirtualKey.F7;
                case "F8"                          : return VirtualKey.F8;
                case "F9"                          : return VirtualKey.F9;
                case "F10"                         : return VirtualKey.F10;
                case "F11"                         : return VirtualKey.F11;
                case "F12"                         : return VirtualKey.F12;
                case "F13"                         : return VirtualKey.F13;
                case "F14"                         : return VirtualKey.F14;
                case "F15"                         : return VirtualKey.F15;
                case "F16"                         : return VirtualKey.F16;
                case "F17"                         : return VirtualKey.F17;
                case "F18"                         : return VirtualKey.F18;
                case "F19"                         : return VirtualKey.F19;
                case "F20"                         : return VirtualKey.F20;
                case "F21"                         : return VirtualKey.F21;
                case "F22"                         : return VirtualKey.F22;
                case "F23"                         : return VirtualKey.F23;
                case "F24"                         : return VirtualKey.F24;
                case "NavigationView"              : return VirtualKey.NavigationView;
                case "NavigationMenu"              : return VirtualKey.NavigationMenu;
                case "NavigationUp"                : return VirtualKey.NavigationUp;
                case "NavigationDown"              : return VirtualKey.NavigationDown;
                case "NavigationLeft"              : return VirtualKey.NavigationLeft;
                case "NavigationRight"             : return VirtualKey.NavigationRight;
                case "NavigationAccept"            : return VirtualKey.NavigationAccept;
                case "NavigationCancel"            : return VirtualKey.NavigationCancel;
                case "NumberKeyLock"               : return VirtualKey.NumberKeyLock;
                case "Scroll"                      : return VirtualKey.Scroll;
                case "LeftShift"                   : return VirtualKey.LeftShift;
                case "RightShift"                  : return VirtualKey.RightShift;
                case "LeftControl"                 : return VirtualKey.LeftControl;
                case "RightControl"                : return VirtualKey.RightControl;
                case "LeftMenu"                    : return VirtualKey.LeftMenu;
                case "RightMenu"                   : return VirtualKey.RightMenu;
                case "GoBack"                      : return VirtualKey.GoBack;
                case "GoForward"                   : return VirtualKey.GoForward;
                case "Refresh"                     : return VirtualKey.Refresh;
                case "Stop"                        : return VirtualKey.Stop;
                case "Search"                      : return VirtualKey.Search;
                case "Favorites"                   : return VirtualKey.Favorites;
                case "GoHome"                      : return VirtualKey.GoHome;
                case "GamepadA"                    : return VirtualKey.GamepadA;
                case "GamepadB"                    : return VirtualKey.GamepadB;
                case "GamepadX"                    : return VirtualKey.GamepadX;
                case "GamepadY"                    : return VirtualKey.GamepadY;
                case "GamepadRightShoulder"        : return VirtualKey.GamepadRightShoulder;
                case "GamepadLeftShoulder"         : return VirtualKey.GamepadLeftShoulder;
                case "GamepadLeftTrigger"          : return VirtualKey.GamepadLeftTrigger;
                case "GamepadRightTrigger"         : return VirtualKey.GamepadRightTrigger;
                case "GamepadDPadUp"               : return VirtualKey.GamepadDPadUp;
                case "GamepadDPadDown"             : return VirtualKey.GamepadDPadDown;
                case "GamepadDPadLeft"             : return VirtualKey.GamepadDPadLeft;
                case "GamepadDPadRight"            : return VirtualKey.GamepadDPadRight;
                case "GamepadMenu"                 : return VirtualKey.GamepadMenu;
                case "GamepadView"                 : return VirtualKey.GamepadView;
                case "GamepadLeftThumbstickButton" : return VirtualKey.GamepadLeftThumbstickButton;
                case "GamepadRightThumbstickButton": return VirtualKey.GamepadRightThumbstickButton;
                case "GamepadLeftThumbstickUp"     : return VirtualKey.GamepadLeftThumbstickUp;
                case "GamepadLeftThumbstickDown"   : return VirtualKey.GamepadLeftThumbstickDown;
                case "GamepadLeftThumbstickRight"  : return VirtualKey.GamepadLeftThumbstickRight;
                case "GamepadLeftThumbstickLeft"   : return VirtualKey.GamepadLeftThumbstickLeft;
                case "GamepadRightThumbstickUp"    : return VirtualKey.GamepadRightThumbstickUp;
                case "GamepadRightThumbstickDown"  : return VirtualKey.GamepadRightThumbstickDown;
                case "GamepadRightThumbstickRight" : return VirtualKey.GamepadRightThumbstickRight;
                case "GamepadRightThumbstickLeft"  : return VirtualKey.GamepadRightThumbstickLeft;
            }

            throw new ArgumentOutOfRangeException();
        }

        #endregion
    }
}
