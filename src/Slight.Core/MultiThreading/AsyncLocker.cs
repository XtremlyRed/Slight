using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Slight.Core
{
    public sealed class AsyncLocker
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly SemaphoreSlim locker;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int maxCount;
        public AsyncLocker(int initialCount)
        {
            locker = new SemaphoreSlim(initialCount);
        }

        public AsyncLocker(int initialCount, int maxCount)
        {
            if (initialCount > maxCount)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCount));
            }
            this.maxCount = maxCount;
            locker = new SemaphoreSlim(initialCount, maxCount);
        }

        public WaitHandle WaitHandle => locker.AvailableWaitHandle;

        public int Release()
        {
            if (maxCount > 0)
            {
                if (locker.CurrentCount >= maxCount)
                {
                    return 0;
                }
            }
            return locker.Release();
        }

        public int Release(int releaseCount)
        {
            if (maxCount > 0)
            {
                int count = locker.CurrentCount + releaseCount;
                if (count > maxCount)
                {
                    return 0;
                }
            }

            return locker.Release(releaseCount);
        }
        public bool Wait(TimeSpan timeout)
        {
            return locker.Wait(timeout);
        }

        public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
        {
            return locker.Wait(timeout, cancellationToken);
        }

        public bool Wait(int timeout_Ms)
        {
            return locker.Wait(timeout_Ms);
        }

        public bool Wait(int timeout_Ms, CancellationToken cancellationToken)
        {
            return locker.Wait(timeout_Ms, cancellationToken);
        }

        public void Wait()
        {
            locker.Wait();
        }

        public void Wait(CancellationToken cancellationToken)
        {
            locker.Wait(cancellationToken);
        }

        public Task WaitAsync()
        {
            return locker.WaitAsync();
        }

        public Task<bool> WaitAsync(int timeout_Ms)
        {
            return locker.WaitAsync(timeout_Ms);
        }

        public Task<bool> WaitAsync(TimeSpan timeout)
        {
            return locker.WaitAsync(timeout);
        }

        public Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            return locker.WaitAsync(timeout, cancellationToken);
        }

        public Task<bool> WaitAsync(int timeout_Ms, CancellationToken cancellationToken)
        {
            return locker.WaitAsync(timeout_Ms, cancellationToken);
        }

        public Task WaitAsync(CancellationToken cancellationToken)
        {
            return locker.WaitAsync(cancellationToken);
        }
    }
}
