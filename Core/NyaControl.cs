using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nyantilities.Core
{
    public class NyaControl : UserControl, INotifyPropertyChanged
    {
        #region Methods

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;
            
            storage = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String property)
        {
            if (HasListeners())
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        protected bool HasListeners()
        {
            return PropertyChanged != null;
        }

        #endregion
    }
}
