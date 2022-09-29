using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Slight.Core
{
    /// <summary>
    /// cancel event handle
    /// </summary>
    public struct CancellationHandle
    {
        /// <summary>
        /// create a new cancel handle
        /// </summary>
        public CancellationHandle()
        {
            ParallelCancel = false;
        }

        /// <summary>
        /// parallel cancel
        /// </summary>
        public bool ParallelCancel { get; set; }

        /// <summary>
        ///  counter map
        /// </summary>
        public IReadOnlyDictionary<string, int> Counter => cancelCallbacker.ToReadOnlyDictionary(i => i.Key, i => i.Value.Count);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private ConcurrentDictionary<string, Collection<Action>> cancelCallbacker = new();

        internal void Dispose()
        {
            cancelCallbacker?.ForEach(i => i.Value?.Clear());
            cancelCallbacker?.Clear();
            cancelCallbacker = null;
        }

        /// <summary>
        /// register cancel callback by <paramref name="subscriber"/>
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="cancelCallback"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public CancellationHandle Register(string subscriber, Action cancelCallback)
        {
            if (subscriber is null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            if (cancelCallback is null)
            {
                throw new ArgumentNullException(nameof(cancelCallback));
            }

            if (cancelCallbacker is null)
            {
                return this;
            }

            if (cancelCallbacker.TryGetValue(subscriber, out Collection<Action> collection) == false)
            {
                cancelCallbacker[subscriber] = collection = new Collection<Action>();
            }

            collection.Add(cancelCallback);

            return this;
        }

        /// <summary>
        ///  unregister  cancel callback by <paramref name="subscriber"/>
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public CancellationHandle Unregister(string subscriber)
        {
            if (subscriber is null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            if (cancelCallbacker is null)
            {
                return this;
            }

            cancelCallbacker.TryRemove(subscriber, out Collection<Action> actions);

            actions?.Clear();

            return this;
        }

        /// <summary>
        ///  clear all cancel callback
        /// </summary>
        public void Clear()
        {
            cancelCallbacker?.ForEach(i =>
            {
                i.Value?.Clear();
            });

            cancelCallbacker?.Clear();
        }

        /// <summary>
        /// invoke cancel callback
        /// </summary>
        public void Cancel()
        {
            if (ParallelCancel)
            {
                cancelCallbacker?.AsParallel()?.ForEach(item =>
                {
                    item.Value?.ForEach(i => i?.Invoke());
                });
                return;
            }

            cancelCallbacker?.ForEach(item =>
            {
                item.Value?.ForEach(i => i?.Invoke());
            });

        }
    }
}
