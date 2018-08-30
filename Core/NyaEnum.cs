using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nyantilities.Core
{
    public class NyaEnum<T>
    {
        protected readonly T value;

        protected NyaEnum(T value)
        {
            this.value = value;
        }

        public static implicit operator T(NyaEnum<T> @enum)
        {
            return @enum.value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
