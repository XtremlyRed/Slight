using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Slight.Core
{
    /// <summary>
    ///  class of <see cref="ReadOnlyDictionary{TKey, TValue}"/>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly Dictionary<TKey, TValue> collection;

        /// <summary>
        /// get value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get => collection[key];
            internal set => collection[key] = value;
        }

        internal ReadOnlyDictionary()
        {
            collection = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// initialize readonly dictionary by exist dictionary
        /// </summary>
        /// <param name="keyValues"></param>
        public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> keyValues) : this()
        {
            if (keyValues is null)
            {
                return;
            }

            foreach (KeyValuePair<TKey, TValue> kvp in keyValues)
            {
                collection[kvp.Key] = kvp.Value;
            }
        }

        /// <summary>
        /// all key
        /// </summary>
        public IEnumerable<TKey> Keys => collection.Keys;

        /// <summary>
        /// all value
        /// </summary>
        public IEnumerable<TValue> Values => collection.Values;

        /// <summary>
        /// count
        /// </summary>
        public int Count => collection.Count;

        /// <summary>
        /// contains key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return collection.ContainsKey(key);
        }

        /// <summary>
        /// <see cref="GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        /// <summary>
        /// <see cref="TryGetValue(TKey, out TValue)"/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return collection.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }
    }
}
