using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyantilities.Collections
{
    public class NotifyingList<T> : List<T>, INotifyCollectionChanged
    {
        #region Constructor

        public NotifyingList()
            : base ()
        {

        }

        public NotifyingList(int capacity)
            : base(capacity)
        {

        }

        public NotifyingList(IEnumerable<T> collection)
            : base(collection)
        {

        }

        #endregion

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void NotifyCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion

        #region Notifying Methods

        public void NotifyAdd(T item)
        {
            Add(item);
            NotifyCollectionChanged();
        }

        public void NotifyAddRange(IEnumerable<T> items)
        {
            AddRange(items);
            NotifyCollectionChanged();
        }

        public void NotifyInsert(int index, T item)
        {
            Insert(index, item);
            NotifyCollectionChanged();
        }

        public void NotifyRemove(T item)
        {
            Remove(item);
            NotifyCollectionChanged();
        }

        public void NotifyRemoveAt(int index)
        {
            RemoveAt(index);
            NotifyCollectionChanged();
        }

        public void NotifyReplace(int index, T item)
        {
            this[index] = item;
            NotifyCollectionChanged();
        }

        public void NotifyClear()
        {
            Clear();
            NotifyCollectionChanged();
        }

        #endregion
    }
}
