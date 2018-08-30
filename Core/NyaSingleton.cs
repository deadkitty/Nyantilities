using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nyantilities.Core
{
    /// <summary>
    /// <para>class that takes care over the singleton behavior, when Instance is first called, it creates a new object of the child class, </para>
    /// <para>after that, it sets a new ReturnFunction that doesnt create an instance anymore, and finally returns the class. </para>
    /// <para>with the destroy instance method the instance is set back to null and another call of Instance will create a new Instance.</para>
    /// <para>to create an instance, the Childs class first constructor must be a parameterless constructor (no constructor is also fine)</para>
    /// </summary>
    public abstract class NyaSingleton<T> where T : class
    {
        private static T sInstance = null;
        private static Func<T> sReturnInstance = CreateAndReturnInstance;

        public static T Instance
        {
            get
            {
                return sReturnInstance();
            }
        }

        private static T CreateInstance()
        {
            ConstructorInfo cInfo = typeof(T).GetTypeInfo().DeclaredConstructors.First();

            return cInfo.Invoke(null) as T;
        }

        private static T CreateAndReturnInstance()
        {
            sInstance = CreateInstance();
            sReturnInstance = ReturnInstance;
            return sInstance;
        }

        private static T ReturnInstance()
        {
            return sInstance;
        }

        public static void DestroyInstance()
        {
            sInstance = null;
            sReturnInstance = CreateAndReturnInstance;
        }
    }
}