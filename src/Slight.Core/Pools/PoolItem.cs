using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Slight.Core.Pools
{

    /// <summary>
    /// object pool item status
    /// </summary>
    public enum PoolItemStatus
    {
        /// <summary>
        /// free
        /// </summary>
        Free,

        /// <summary>
        /// busy
        /// </summary>
        Busy,
    }

    /// <summary>
    ///  a struct of <see cref="PoolItem{Target}"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    [DebuggerDisplay("{Value}")]
    public struct PoolItem<Target> : IDisposable
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal int counter;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DateTime lastRentTime;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DateTime lastReturnTime;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal PoolItemStatus status;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IPoolManager<Target> poolManager;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Target value;

        /// <summary>
        /// create pool item
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="id"></param>
        /// <param name="poolManager"></param>
        internal PoolItem(Target targetValue, int id, IPoolManager<Target> poolManager)
        {
            value = targetValue;
            this.poolManager = poolManager;
            status = PoolItemStatus.Free;
            counter = 0;
            Id = id;
            lastRentTime = lastReturnTime = new DateTime();
        }

        /// <summary>
        /// valid value
        /// </summary>
        public Target Value
        {
            get
            {
                if (status == PoolItemStatus.Busy)
                {
                    return value;
                }

                throw new Exception("The object has been returned and cannot be used");
            }
        }


        /// <summary>
        /// Count of uses
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Counter => counter;

        /// <summary>
        /// current status
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PoolItemStatus Status => status;

        /// <summary>
        /// item id
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly int Id;

        /// <summary>
        /// last rent time
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DateTime LastRentTime => lastRentTime;


        /// <summary>
        /// last return time
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DateTime LastReturnTime => lastReturnTime;

        /// <summary>
        /// auto return to pool
        /// </summary> 
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IDisposable.Dispose()
        {

            poolManager.Return(this);
        }

        /// <summary>
        ///   Return to Pool
        /// </summary>
        public void Return()
        {
            poolManager.Return(this);
        }

        /// <summary>
        /// to  <typeparamref name="Target"/> object 
        /// </summary>
        /// <param name="poolItem"></param>
        public static implicit operator Target(PoolItem<Target> poolItem)
        {
            return poolItem.Value;
        }
    }


    /// <summary>
    /// interface of target object can reset value
    /// </summary>
    public interface IItemResettable
    {
        /// <summary>
        /// reset value
        /// </summary>
        void Reset();
    }
}
