using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Slight.Core
{
    /// <summary>
    ///  class of <see cref="ReadOnlyList{Target}"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyList<Target> : IReadOnlyList<Target>, IReadOnlyCollection<Target>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly IList<Target> _collection;

        /// <summary>
        /// initialize a <see cref="ReadOnlyList{Target}"/>
        /// </summary>
        /// <param name="collection"></param>
        public ReadOnlyList(IEnumerable<Target> collection)
        {
            _collection = new List<Target>(collection);
        }

        /// <summary>
        /// get item
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Target this[int index] => _collection[index];

        /// <summary>
        /// list count
        /// </summary>
        public int Count => _collection.Count;


        /// <summary>
        /// <see cref="GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Target> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }
}
