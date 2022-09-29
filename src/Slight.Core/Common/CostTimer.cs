
using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Slight.Core
{
    /// <summary>
    /// <para> class of  <see cref="CostTimer"/></para>
    /// <para> a timer helper class of the execute time</para>
    /// </summary>
    public sealed class CostTimer : IDisposable
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly ConcurrentDictionary<object, CostTimer> timerMapper = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Stopwatch stopwatch;



        private CostTimer()
        {
            stopwatch = Stopwatch.StartNew();
        }
        /// <summary>
        /// dispose this timer
        /// </summary>
        ~CostTimer()
        {
            Dispose();
        }
        /// <summary>
        /// dispose this timer
        /// </summary>
        public void Dispose()
        {
            Stop();
        }


        private void Stop()
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }
        }

        /// <summary>
        /// stop this timer and return  execute  use time
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeSpan()
        {
            Stop();

            return stopwatch.Elapsed;
        }

        /// <summary>
        ///  stop this timer and return  execute  use time
        /// </summary>
        /// <returns></returns>
        public long GetTotalMilliseconds()
        {
            Stop();

            return stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Timer:{stopwatch.ElapsedMilliseconds} ms";
        }

        /// <summary>
        /// start a new timer
        /// </summary>
        /// <returns></returns>
        public static CostTimer StartNew()
        {
            return new CostTimer();
        }

        /// <summary>
        /// set a token  and  start a new timer by token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static CostTimer SetTimer(object token)
        {
            return token is null
                ? throw new ArgumentNullException(nameof(token))
                : timerMapper.TryGetValue(token, out CostTimer timer)
                ? throw new ArgumentException($"Token:{token} registered", nameof(token))
                : (timerMapper[token] = new CostTimer());
        }

        /// <summary>
        /// get used time by token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="removeTokenAfterRead"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static TimeSpan GetTimeSpan(object token, bool removeTokenAfterRead = true)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (!timerMapper.TryGetValue(token, out CostTimer timer))
            {
                throw new NotSupportedException($"Token:{token} not registered ");
            }

            if (removeTokenAfterRead)
            {
                timerMapper.TryRemove(token, out CostTimer _);
            }

            return timer.GetTimeSpan();
        }

        /// <summary>
        /// get used time by token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="removeTokenAfterRead"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static double GetTotalMilliseconds(object token, bool removeTokenAfterRead = true)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (!timerMapper.TryGetValue(token, out CostTimer timer))
            {
                throw new NotSupportedException($"Token:{token} not registered ");
            }

            timer.Stop();

            if (removeTokenAfterRead)
            {
                timerMapper.TryRemove(token, out CostTimer _);
            }

            return timer.GetTotalMilliseconds();
        }


        /// <summary>
        /// run an action and return a timer
        /// </summary>
        /// <param name="action">action callback</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static CostTimer Run(Action action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            CostTimer timer = CostTimer.StartNew();
            try
            {
                action();
            }
            finally
            {
                timer.Stop();
            }

            return timer;
        }
    }
}