using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nyantilities.Core
{
    public abstract class NyaObservable : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        protected bool SetProperty<T>(ref T storage, T value, params String[] properties)
        {
            if (Equals(storage, value)) return false;

            storage = value;

            foreach (String property in properties)
            {
                NotifyPropertyChanged(property);
            }

            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String property)
        {
            if (HasListeners())
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        protected void NotifyPropertiesChanged(params String[] properties)
        {
            if (HasListeners())
            {
                foreach (String property in properties)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
                }
            }
        }

        protected bool HasListeners()
        {
            return PropertyChanged != null;
        }

        #endregion
    }
}
