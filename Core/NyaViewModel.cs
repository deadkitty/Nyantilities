using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyantilities.Core
{
    public abstract class NyaViewModel : NyaObservable
    {
        #region Properties

        private bool initialized = false;
        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }
        
        #endregion

        #region Save/Load State

        public virtual void SaveState(SaveStateEventArgs args)
        {

        }

        public virtual void LoadState(LoadStateEventArgs args)
        {
            
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

        #endregion
    }
}
