using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyantilities.Collections
{
    public static class ListExtensions
    {
        #region ListExtensions

        public static T PopFront<T>(this List<T> list)
        {
            T item = list.First();
            list.RemoveAt(0);
            return item;
        }

        public static T PopBack<T>(this List<T> list)
        {
            T item = list.Last();
            list.RemoveAt(list.Count - 1);
            return item;
        }

        public static void PushFront<T>(this List<T> list, T item)
        {
            list.Insert(0, item);
        }

        public static void PushBack<T>(this List<T> list, T item)
        {
            list.Add(item);
        }

        public static List<T> PopRange<T>(this List<T> list, int startIndex, int count)
        {
            List<T> items = list.GetRange(startIndex, count);
            list.RemoveRange(startIndex, count);

            return items;
        }

        public static List<T> Swap<T>(this List<T> list, int index1, int index2)
        {
            T hv = list[index1];
            list[index1] = list[index2];
            list[index2] = hv;
            
            return list;
        }

        public static List<T> Swap<T>(this List<T> list, T item1, T item2)
        {
            return list.Swap(list.IndexOf(item1), list.IndexOf(item2));
        }

        public static List<T> Randomize<T>(this List<T> list, int seed = -1)
        {
            Random rand = new Random(seed == -1 ? (int)DateTime.Now.Ticks : seed);

            for (int i = 0; i < list.Count; i++)
            {
                int newIndex = rand.Next(list.Count);

                T hv = list[i];
                list[i] = list[newIndex];
                list[newIndex] = hv;
            }

            return list;
        }

        public static void Sort1<T>(this List<T> list, IComparer<T> comparer)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    int value = comparer.Compare(list[i], list[j]);

                    if(value > 0)
                    {
                        list.Swap(i, j);
                    }
                }
            }
        }

        // 4 2 7 3 5 1 9 3 5 6 8
        // 2 4 7 3 5 1 9 3 5 6 8
        // 1 4 7 3 5 2 9 3 5 6 8
        // 1 2 7 3 5 4 9 3 5 6 8
        // 1 2 3 7 5 4 9 3 5 6 8
        // 1 2 3 3 5 4 9 7 5 6 8
        // 1 2 3 3 4 5 9 7 5 6 8
        // 1 2 3 3 4 5 5 7 9 6 8
        // 1 2 3 3 4 5 5 6 9 7 8
        // 1 2 3 3 4 5 5 6 7 9 8
        // 1 2 3 3 4 5 5 6 7 8 9

        #endregion

        #region NotifyingListExtensions

        public static T PopFront<T>(this NotifyingList<T> list)
        {
            T item = list.First();
            list.NotifyRemoveAt(0);
            return item;
        }

        public static T PopBack<T>(this NotifyingList<T> list)
        {
            T item = list.Last();
            list.NotifyRemoveAt(list.Count - 1);
            return item;
        }

        public static void PushFront<T>(this NotifyingList<T> list, T item)
        {
            list.NotifyInsert(0, item);
        }

        public static void PushBack<T>(this NotifyingList<T> list, T item)
        {
            list.NotifyAdd(item);
        }

        #endregion
    }
}
