using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;


namespace Slight.Core
{
    /// <summary>
    /// async handle
    /// </summary>
    public abstract class AsyncHandle : IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private ManualResetEventSlim locker = new(false);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private Stopwatch stopwatch = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private Exception currentException;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool isDisposed;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// create a new async handle by <paramref name="handleName"/> and <paramref name="cancellationToken"/>
        /// </summary>
        /// <param name="handleName"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected AsyncHandle(string handleName, CancellationToken cancellationToken = default)
        {
            Name = handleName ?? throw new ArgumentNullException(nameof(handleName));
            Token = cancellationToken;
            CancellationHandle = new CancellationHandle();
            tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        }

        /// <summary>
        /// cancel event handle
        /// </summary>
        public CancellationHandle CancellationHandle { get; }

        /// <summary>
        /// token
        /// </summary>
        public CancellationToken Token { get; }

        /// <summary>
        /// handle name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// tag info
        /// </summary>
        public virtual object Tag { get; set; }


        /// <summary>
        /// waithandle
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WaitHandle Handle => locker?.WaitHandle ?? default;

        /// <summary>
        /// cost time
        /// </summary>
        public TimeSpan Elapsed => stopwatch?.Elapsed ?? default;

        /// <summary>
        /// cancel status
        /// </summary>
        public bool IsCancellationRequested => Token.IsCancellationRequested;

        /// <summary>
        /// cancel async handle
        /// </summary>
        public virtual void Cancel()
        {
            try
            {
                DisposeThrower();
                tokenSource?.Cancel();
            }
            finally
            {
                stopwatch?.Stop();
            }
        }

        /// <summary>
        /// wait async handle down in  <paramref name="millisecondsTimeout"/>
        /// </summary>

        public virtual bool WaitDone(int millisecondsTimeout)
        {
            DisposeThrower();
            try
            {

                stopwatch?.Start();
                bool waitResult = locker.Wait(millisecondsTimeout, tokenSource.Token);

                return waitResult;
            }
            catch (OperationCanceledException ee)
            {
                currentException = new Exception(ee.GetBaseException().Message);
                CancellationHandle.Cancel();
            }
            catch (Exception e)
            {
                currentException = new Exception(e.GetBaseException().Message);
            }
            finally
            {
                stopwatch?.Stop();
            }

            ExceptionThrower();

            return false;
        }

        /// <summary>
        /// wait async handle down
        /// </summary>
        public virtual void WaitDone()
        {
            DisposeThrower();

            try
            {
                if (stopwatch.IsRunning == false)
                {
                    stopwatch.Start();
                }
                locker.Wait(tokenSource.Token);
            }
            catch (OperationCanceledException ee)
            {
                currentException = new Exception(ee.GetBaseException().Message);
                CancellationHandle.Cancel();
            }
            catch (Exception e)
            {
                currentException = new Exception(e.GetBaseException().Message);
            }
            finally
            {
                stopwatch?.Stop();
            }

            ExceptionThrower();
        }

        /// <summary>
        /// reset the wait handle
        /// </summary>
        public virtual void Set()
        {
            DisposeThrower();

            if (locker != null && locker.IsSet == false)
            {
                locker.Set();
            }

            stopwatch?.Stop();
        }

        /// <summary>
        ///  throw when has exception
        /// </summary>
        /// <param name="exception"></param>
        public virtual void ThrowException(Exception exception)
        {
            DisposeThrower();
            currentException = exception;
            locker?.Set();
        }

        private void ExceptionThrower()
        {
            if (currentException != null)
            {
                if (Name is null)
                {
                    throw currentException;
                }
                throw new Exception(Name, currentException);
            }
        }

        private void DisposeThrower()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(Name);
            }
        }


        /// <summary>
        /// dispose object
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }
            isDisposed = true;

            locker?.Dispose();
            locker = null;

            tokenSource?.Dispose();
            tokenSource = null;

            CancellationHandle.Dispose();

            stopwatch?.Stop();
            stopwatch = null;

            Dispose(isDisposed);
        }


        /// <summary>
        /// release async handle
        /// </summary>
        ~AsyncHandle()
        {
            Dispose();
        }


        /// <summary>
        /// disposing
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {

        }


        /// <summary>
        /// wait all async handle down
        /// </summary>
        /// <param name="handles"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool WaitAll(params AsyncHandle[] handles)
        {
            if (handles is null)
            {
                throw new ArgumentNullException(nameof(handles));
            }

            WaitHandle[] validHandles = handles.Select(i => i.Handle).Where(i => i != null).ToArray();

            if (validHandles.Length == 0)
            {
                return false;
            }

            bool waitResult = WaitHandle.WaitAll(validHandles);

            return waitResult;
        }


        /// <summary>
        /// wait all async handle down
        /// </summary>
        /// <param name="handles"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="throwTimeoutException"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="TimeoutException"></exception>
        public static bool WaitAll(AsyncHandle[] handles, int millisecondsTimeout, bool throwTimeoutException = true)
        {
            if (handles is null)
            {
                throw new ArgumentNullException(nameof(handles));
            }

            WaitHandle[] validHandles = handles.Select(i => i.Handle).Where(i => i != null).ToArray();

            if (validHandles.Length == 0)
            {
                return false;
            }

            bool waitResult = WaitHandle.WaitAll(validHandles, millisecondsTimeout);

            return waitResult == false && throwTimeoutException ? throw new TimeoutException() : waitResult;
        }


        /// <summary>
        /// create new async handle
        /// </summary>
        /// <param name="sharedName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static AsyncHandle Shared(string sharedName, CancellationToken token = default)
        {
            return new DefaultAsyncHandle(sharedName, token);
        }


        private class DefaultAsyncHandle : AsyncHandle
        {
            public DefaultAsyncHandle(string handleName, CancellationToken cancellationToken) : base(handleName, cancellationToken)
            {
            }
        }
    }
}
