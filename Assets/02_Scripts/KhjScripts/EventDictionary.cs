using LJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace khj
{
    public class EventDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public delegate void DictionaryChanged();
        public event DictionaryChanged OnAdd;
        public event DictionaryChanged OnRemove;

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnAdd?.Invoke();
        }

        public new bool Remove(TKey key)
        {
            var removed = base.Remove(key);
            if (removed)
            {
                OnRemove?.Invoke();
            }
            return removed;
        }
    }
}
