using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nyantilities.Commands
{
    public class RelayCommand<T> : ICommand
    {
        private bool enabled;
        private readonly Action<T> execute;
        private readonly Predicate<object> canExecute;
     
        public bool IsEnabled
        {
            get => enabled;
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
            enabled = true;
        }

        public RelayCommand(Action<T> execute, Predicate<object> canExecute)
        {
            enabled = true;
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }        
    }
}
