using Nyantilities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyantilities.ViewModel
{
    public static class ViewModelProvider
    {
        private static Dictionary<Type, NyaViewModel> vmDictionary = new Dictionary<Type, NyaViewModel>();

        /// <summary>
        /// returns a new ViewModel specified by the given type
        /// </summary>
        /// <typeparam name="TViewModel">type of the viewmodel that will be returned</typeparam>
        /// <param name="isStatic">if true, only one instance is instanciated and another call will just return the old instance, otherwise a new viewmodel is created every time</param>
        /// <returns></returns>
        public static TViewModel GetViewModel<TViewModel>(bool isStatic = true) where TViewModel : NyaViewModel, new()
        {
            if(isStatic)
            {
                TViewModel vm = null;

                if (vmDictionary.ContainsKey(typeof(TViewModel)))
                {
                    vm = vmDictionary[typeof(TViewModel)] as TViewModel;
                }
                else
                {
                    vm = new TViewModel();
                    vmDictionary[typeof(TViewModel)] = vm;
                }

                return vm as TViewModel;
            }
            else
            {
                return new TViewModel();
            }
        }
    }
}
