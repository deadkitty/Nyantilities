using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nyantilities
{
    public static class DebugHelper
    {
#if DEBUG

        private static int MessageCount = 0;

        public static void WriteLine(String message)
        {
            Debug.WriteLine(String.Format("{0} {1}", ++MessageCount, message));
        }

        public static void WriteLine<T>(String message = null, [CallerMemberName] String callingMethod = null)
        {
            Debug.WriteLine("{0} {1}.{2}: {3}", ++MessageCount, typeof(T).Name, callingMethod, message);
        }

        public static void MethodWriteLine<T>([CallerMemberName] String callingMethod = null)
        {
            Debug.WriteLine("{0} {1}.{2}", ++MessageCount, typeof(T).Name, callingMethod);
        }

#else
        
        public static void WriteLine(String message)
        {
        }

        public static void WriteLine<T>(String message, [CallerMemberName] String callingMethod = null)
        {
        }

        public static void MethodWriteLine<T>([CallerMemberName] String callingMethod = null)
        {
        }

#endif
    }
}