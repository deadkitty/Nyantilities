using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyantilities.Collections
{
    /// <summary>
    /// can store multible lists with items that are grouped by a specific key
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    public class GroupedList<Key, Value> : Dictionary<Key, GroupedList<Key, Value>.ValueList>
    {
        private int numItems = 0;
        public int NumItems
        {
            get { return numItems; }
            set { numItems = value; }
        }
        
        public class ValueList : List<Value>
        {
            /// <summary>
            /// The Key of this group.
            /// </summary>
            public Key Key { get; private set; }

            public ValueList(Key key)
            {
                this.Key = key;
            }
        }

        public void AddGroups(IEnumerable<Key> keys)
        {
            foreach (Key key in keys)
            {
                if(!ContainsKey(key))
                {
                    Add(key, new ValueList(key));
                }
            }
        }

        public void InsertValues(IEnumerable<Value> values, Func<Value, Key> keyGenerator, CultureInfo ci = null, bool sort = true)
        {
            foreach (Value value in values)
            {
                Key key = keyGenerator(value);
                
                this[key].Add(value);
            }

            numItems += values.Count();
        }

        public List<Value> GetAllItems()
        {
            return Values.SelectMany(x => x).ToList();
        }


        public List<ValueList> AsList()
        {
            List<ValueList> list = new List<ValueList>();

            foreach (Key key in Keys)
            {
                list.Add(this[key]);
            }

            return list;
        }
    }
}
