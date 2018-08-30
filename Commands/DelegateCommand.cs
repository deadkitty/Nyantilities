using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nyantilities.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action action;
        private bool enabled;

        public DelegateCommand(Action action)
        {
            this.action = action;
            enabled = true;
        }

        public bool IsEnabled
        {
            get => enabled;
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    OnCanExecuteChanged();
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return enabled;
        }

        public void Execute(object parameter)
        {
            action();
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
