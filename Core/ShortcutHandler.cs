using Nyantilities.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Nyantilities.Core
{
    public class ShortcutHandler : IDisposable
    {
        #region Fields

        protected Dictionary<VirtualKey, ICommand> Commands = new Dictionary<VirtualKey, ICommand>();

        #endregion

        #region Constructor

        public ShortcutHandler()
        {
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        #endregion

        #region Key Down

        public void RegisterCommand(VirtualKey key, ICommand command)
        {
            Commands.Add(key, command);
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (Commands.ContainsKey(args.VirtualKey))
            {
                if (Commands[args.VirtualKey].CanExecute(null))
                {
                    Commands[args.VirtualKey].Execute(null);
                }
            }
        }

        //public abstract void KeyDown(KeyEventArgs args);
        
        #endregion
        
        #region IDisposable

        public void Dispose()
        {
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
        }

        #endregion
    }
}
