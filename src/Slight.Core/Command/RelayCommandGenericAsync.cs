using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
namespace Slight.Core
{
    /// <summary>
    /// a class of <see cref="RelayCommandAsync{TParameter}"/>
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public sealed class RelayCommandAsync<TParameter> : ICommandAsync<TParameter>, IDisposable
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Collection<Func<TParameter, Task>> executeFuncs = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Func<TParameter, bool> canExecuteFunc;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly Action<Exception> exceptionCallback;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private bool isExecuting;
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
        /// create a new instance
        /// </summary>
        /// <param name="executeFunc">command body</param>
        /// <param name="canExecuteFunc">can execute fnction</param>
        /// <param name="exceptionCallback">exception callbase</param>
        public RelayCommandAsync(Func<TParameter, Task> executeFunc, Func<TParameter, bool> canExecuteFunc = null, Action<Exception> exceptionCallback = null)
        {
            if (executeFunc is null)
            {
                throw new ArgumentNullException(nameof(executeFunc));
            }
            executeFuncs?.Add(executeFunc);
            this.canExecuteFunc = canExecuteFunc;
            this.exceptionCallback = exceptionCallback;
        }

        /// <summary>
        /// Append an other command into the Command body
        /// </summary>
        /// <param name="executeFunc">an other command body</param> 
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommandAsync<TParameter> Append(Func<TParameter, Task> executeFunc)
        {
            if (executeFunc is null)
            {
                throw new ArgumentNullException(nameof(executeFunc));
            }
            executeFuncs?.Add(executeFunc);
            return this;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// can execute of the command
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            if (parameter is not TParameter target)
            {
                return true;
            }

            bool able = CanExecute(target);

            return able;
        }

        /// <summary>
        /// can execute of the command
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(TParameter parameter)
        {
            bool able = canExecuteFunc?.Invoke(parameter) ?? true;

            return able;
        }

        /// <summary>
        /// execute Command
        /// </summary>
        /// <param name="parameter"></param>
        async void ICommand.Execute(object parameter)
        {
            if (parameter is TParameter target)
            {
                await ExecuteAsync(target);
            }
        }

        /// <summary>
        ///  Execute Command
        /// </summary>
        /// <param name="parameter"></param>
        public Task ExecuteAsync(TParameter parameter)
        {
            lock (executeFuncs)
            {
                if (isExecuting || executeFuncs is null || executeFuncs.Count == 0 || CanExecute(parameter) == false)
                {
                    return Task.FromResult(false);
                }

                isExecuting = true;
            }


            Task[] tasks = executeFuncs?.Select(i => i?.Invoke(parameter)).Where(i => i != null).ToArray();

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
