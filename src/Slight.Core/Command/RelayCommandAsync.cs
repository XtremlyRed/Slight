using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Slight.Core
{  /// <summary>
   /// a class of <see cref="RelayCommandAsync"/>
   /// </summary>
    public sealed class RelayCommandAsync : ICommandAsync, IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Collection<Func<Task>> executeFuncs = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Func<bool> canExecuteFunc;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private bool isExecuting;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly Action<Exception> exceptionCallback;
        /// <summary>
        /// 
        /// </summary>
        void IDisposable.Dispose()
        {
            canExecuteFunc = null;
            executeFuncs?.Clear();
            executeFuncs = null;
        }

        /// <summary>
        /// create a new instance of <see cref="RelayCommandAsync"/>
        /// </summary>
        public RelayCommandAsync(Func<Task> executableCommandCallback, Func<bool> canExecuteFunc = null, Action<Exception> exceptionCallback = null)
        {
            if (executableCommandCallback is null)
            {
                throw new ArgumentNullException(nameof(executableCommandCallback));
            }

            executeFuncs?.Add(executableCommandCallback);
            this.canExecuteFunc = canExecuteFunc;
            this.exceptionCallback = exceptionCallback;
        }

        /// <summary>
        /// create a new instance of <see cref="RelayCommandAsync"/>
        /// </summary>
        public RelayCommandAsync(Func<Task> executableCommandCallback, Action<Exception> exceptionCallback = null)
        {
            if (executableCommandCallback is null)
            {
                throw new ArgumentNullException(nameof(executableCommandCallback));
            }
            executeFuncs?.Add(executableCommandCallback);
            canExecuteFunc = null;
            this.exceptionCallback = exceptionCallback;
        }

        /// <summary>
        /// Append an other command into the Command body
        /// </summary>
        /// <param name="executableCommandCallback">an other command body</param> 
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommandAsync Append(Func<Task> executableCommandCallback)
        {
            if (executableCommandCallback == null)
            {
                throw new ArgumentNullException(nameof(executableCommandCallback));
            }
            executeFuncs?.Add(executableCommandCallback);
            return this;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Can Execute Function
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// Can Execute Function
        /// </summary> 
        /// <returns></returns>
        public bool CanExecute()
        {
            return canExecuteFunc?.Invoke() ?? true;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="parameter"></param>
        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync();
        }

        /// <summary>
        /// Execute
        /// </summary>
        public Task ExecuteAsync()
        {
            lock (executeFuncs)
            {
                if (isExecuting || executeFuncs is null || executeFuncs.Count == 0 || CanExecute() == false)
                {
                    return Task.FromResult(false);
                }

                isExecuting = true;
            }

            Task[] tasks = executeFuncs?.Select(i => i?.Invoke()).Where(i => i != null).ToArray();

            if (tasks is null || tasks.Length == 0)
            {
                return Task.FromResult(false);
            }

            Task executeTask = null;

            if (tasks.Length == 1)
            {
                executeTask = tasks[0];
            }
            else
            {
                executeTask = Task.WhenAll(tasks);
            }

            return executeTask.ContinueWith(y =>
            {
                try
                {
                    y.Wait();
                }
                catch (Exception e)
                {
                    exceptionCallback?.Invoke(e);
                }
                finally
                {
                    isExecuting = false;
                }
            });
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}