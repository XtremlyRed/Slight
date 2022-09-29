using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Slight.Core.Pools;

namespace Slight.Core
{
    /// <summary>
    ///  PoolManager
    /// </summary>
    public class PoolManager
    {
        /// <summary>
        /// create default pool 
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public static IPoolManager<Target> Create<Target>(int capacity)
        {
            return new PoolManager<Target>(capacity);
        }
    }






    /// <summary>
    ///  default pool manager of   <typeparamref name="Target"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    public class PoolManager<Target> : IPoolManager<Target>, IDisposable
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int createCounter;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int waitCounter;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SemaphoreSlim semaphore = new(0);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConcurrentStack<PoolItem<Target>> freeObjects = new();

        /// <summary>
        /// the  capacity of pool 
        /// </summary>
        public readonly int Capacity;

        /// <summary>
        /// create new pool
        /// </summary>
        /// <param name="capacity"></param>
        internal PoolManager(int capacity = 0)
        {
            if (capacity <= 0)
            {
                capacity = byte.MaxValue + 1;
            }

            Capacity = capacity;
        }



        bool IPoolManager<Target>.Return(PoolItem<Target> poolItem)
        {
            if (freeObjects.Count + 1 <= Capacity)
            {
                if (poolItem.Value is IItemResettable resettable)
                {
                    resettable.Reset();
                }

                poolItem.status = PoolItemStatus.Free;
                poolItem.lastReturnTime = DateTime.Now;

                freeObjects.Push(poolItem);

                if (waitCounter > 0)
                {
                    semaphore.Release(1);
                }

                return true;
            }

            return false;
        }

        PoolItem<Target> IPoolManager<Target>.Rent()
        {

            if (freeObjects.TryPop(out PoolItem<Target> poolItem) == false)
            {
                if (createCounter >= Capacity)
                {
                    throw new InvalidOperationException("No objects available, the number of objects has exceeded the capacity");
                }

                poolItem = new PoolItem<Target>(OnObjectCreate(), Interlocked.Increment(ref createCounter), this);
            }

            poolItem.counter++;
            poolItem.lastRentTime = DateTime.Now;
            poolItem.status = PoolItemStatus.Busy;

            return poolItem;
        }

        async Task<PoolItem<Target>> IPoolManager<Target>.RentAsync(int waitTimeout_Ms)
        {

        getItem:

            if (freeObjects.TryPop(out PoolItem<Target> poolItem) == false)
            {
                if (createCounter >= Capacity)
                {
                    Interlocked.Increment(ref waitCounter);

                    bool result = await semaphore.WaitAsync(waitTimeout_Ms);

                    Interlocked.Decrement(ref waitCounter);

                    if (result)
                    {
                        goto getItem;
                    }

                    throw new TimeoutException("Getting object timeout");
                }

                poolItem = new PoolItem<Target>(OnObjectCreate(), Interlocked.Increment(ref createCounter), this);
            }

            poolItem.counter++;
            poolItem.lastRentTime = DateTime.Now;
            poolItem.status = PoolItemStatus.Busy;

            return poolItem;
        }


        /// <summary>
        /// method of object creator
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual Target OnObjectCreate()
        {
            return Activator.CreateInstance<Target>();
        }


        /// <summary>
        /// dispose object
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Dispose()
        {
            semaphore?.Dispose();
            semaphore = null;

            freeObjects.Clear();
            freeObjects = null;
        }
    }
}
