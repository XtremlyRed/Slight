
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Slight.Core
{
    /// <summary>
    /// class of  <see cref="EasyTimer"/>
    /// </summary>
    public sealed class EasyTimer : IDisposable
    {


        [DebuggerBrowsable(DebuggerBrowsableState.Never)] public static EasyTimer Shared => new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private System.Timers.Timer timer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Action<object, System.Timers.ElapsedEventArgs> callbackAction2;

        /// <summary>
        /// the status of the Timer
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// create a new Timer instance
        /// </summary>
        public EasyTimer()
        {
            timer = new System.Timers.Timer
            {
                AutoReset = true
            };


            timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        ~EasyTimer()
        {
            Dispose();
        }

        /// <summary>
        /// dispose this timer
        /// </summary>
        public void Dispose()
        {
            if (timer is null)
            {
                return;
            }
            IsRunning = false;


            timer.Elapsed -= Timer_Elapsed;
            timer.Stop();
            timer.Dispose();
            timer = null;
            callbackAction2 = null;
        }


        /// <summary>
        /// UesCallback
        /// </summary>
        /// <param name="callbackAction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public EasyTimer UseCallback(Action<object, System.Timers.ElapsedEventArgs> callbackAction)
        {
            callbackAction2 = callbackAction ?? throw new ArgumentNullException(nameof(callbackAction));
            return this;
        }

        /// <summary>
        /// UseAutoReset
        /// </summary>
        /// <param name="autoReset"></param>
        /// <returns></returns>
        public EasyTimer UseAutoReset(bool autoReset)
        {
            timer.AutoReset = autoReset;
            return this;
        }

        /// <summary>
        /// UseInterval
        /// </summary>
        /// <param name="milliseconds">milliseconds</param>
        /// <returns></returns>
        public EasyTimer UseInterval(int milliseconds)
        {
            timer.Interval = milliseconds;
            return this;
        }
        /// <summary>
        /// UseSynchronizingObject
        /// </summary>
        /// <param name="synchronizingObject"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public EasyTimer UseSynchronizingObject(ISynchronizeInvoke synchronizingObject)
        {
            timer.SynchronizingObject = synchronizingObject ?? throw new ArgumentNullException(nameof(synchronizingObject));
            return this;
        }

        /// <summary>
        /// UseSite
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public EasyTimer UseSite(ISite site)
        {
            timer.Site = site ?? throw new ArgumentNullException(nameof(site));
            return this;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            callbackAction2?.Invoke(sender, e);
        }

        /// <summary>
        /// start the timer
        /// </summary>
        /// <returns></returns>
        public EasyTimer RunAsync()
        {
            timer?.Start();
            IsRunning = true;
            return this;
        }

        /// <summary>
        /// exit the timer
        /// </summary>
        public void Exit()
        {
            IsRunning = false;
            timer?.Stop();
        }
    }
}