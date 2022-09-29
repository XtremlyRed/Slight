
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Slight.Core
{


    /// <summary>
    /// class of <see cref="RelayCommand"/>
    /// </summary>
    public sealed class RelayCommand : ICommand, IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private bool isExecuting;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Collection<Action> executeActions = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Func<bool> canExecuteFunc;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly Action<Exception> exceptionCallback;
        /// <summary>
        /// 
        /// </summary>
        void IDisposable.Dispose()
        {
            canExecuteFunc = null;
            executeActions?.Clear();
            executeActions = null;
        }


        /// <summary>
        /// create a new instance of <see cref="RelayCommand"/>
        /// </summary>
        public RelayCommand(Action executableCommandCallback, Func<bool> canExecuteFunc = null, Action<Exception> exceptionCallback = null)
        {
            if (executableCommandCallback is null)
            {
                throw new ArgumentNullException(nameof(executableCommandCallback));
            }

            executeActions?.Add(executableCommandCallback);
            this.canExecuteFunc = canExecuteFunc;
            this.exceptionCallback = exceptionCallback;
        }


        /// <summary>
        /// create a new instance of <see cref="RelayCommand"/>
        /// </summary>
        public RelayCommand(Action executableCommandCallback)
        {
            if (executableCommandCallback is null)
            {
                throw new ArgumentNullException(nameof(executableCommandCallback));
            }
            executeActions?.Add(executableCommandCallback);
            canExecuteFunc = null;
        }


        /// <summary>
        /// Append an other command into the Command body
        /// </summary>
        /// <param name="executableCommandCallback">an other command body</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand Append(Action executableCommandCallback)
        {
            if (executableCommandCallback == null)
            {
                throw new ArgumentNullException(nameof(executableCommandCallback));
            }
            executeActions?.Add(executableCommandCallback);
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
        void ICommand.Execute(object parameter)
        {
            Execute();
        }


        /// <summary>
        /// Execute
        /// </summary>
        public void Execute()
        {
            lock (executeActions)
            {
                if (executeActions is null || executeActions.Count == 0 || isExecuting || CanExecute() == false)
                {
                    return;
                }

                isExecuting = true;
            }
            try
            {
                executeActions?.ForEach(i => i?.Invoke());
            }
            catch (Exception e)
            {
                if (exceptionCallback is null)
                {
                    throw e;
                }
                exceptionCallback.Invoke(e);
            }
            finally
            {
                isExecuting = false;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///  binding command
        /// </summary>
        /// <param name="commandAction"></param>
        /// <param name="canExecuteAction"></param>
        /// <returns></returns>
        public static RelayCommand Bind(Action commandAction, Func<bool> canExecuteAction = null)
        {
            return new RelayCommand(commandAction, canExecuteAction);
        }

        /// <summary>
        ///  binding command
        /// </summary>
        /// <param name="commandAction"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static RelayCommand Bind(Action commandAction, Action<Exception> exceptionCallback)
        {
            return new RelayCommand(commandAction, () => true, exceptionCallback);
        }

        /// <summary>
        /// binding command
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <param name="commandAction"></param>
        /// <param name="canExecuteAction"></param>
        /// <returns></returns>
        public static RelayCommand<Target> Bind<Target>(Action<Target> commandAction, Func<Target, bool> canExecuteAction = null)
        {
            return new RelayCommand<Target>(commandAction, canExecuteAction);
        }

        /// <summary>
        /// binding command
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <param name="commandAction"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static RelayCommand<Target> Bind<Target>(Action<Target> commandAction, Action<Exception> exceptionCallback)
        {
            return new RelayCommand<Target>(commandAction, t => true, exceptionCallback);
        }

        /// <summary>
        /// binding command
        /// </summary> 
        /// <param name="commandAction"></param>
        /// <param name="canExecuteAction"></param>
        /// <returns></returns>
        public static RelayCommandAsync Bind(Func<Task> commandAction, Func<bool> canExecuteAction = null)
        {
            return new RelayCommandAsync(commandAction, canExecuteAction);
        }

        /// <summary>
        /// binding command
        /// </summary> 
        /// <param name="commandAction"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static RelayCommandAsync Bind(Func<Task> commandAction, Action<Exception> exceptionCallback)
        {
            return new RelayCommandAsync(commandAction, () => true, exceptionCallback);
        }

        /// <summary>
        /// binding command
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <param name="commandAction"></param>
        /// <param name="canExecuteAction"></param>
        /// <returns></returns>
        public static RelayCommandAsync<Target> Bind<Target>(Func<Target, Task> commandAction, Func<Target, bool> canExecuteAction = null)
        {
            return new RelayCommandAsync<Target>(commandAction, canExecuteAction);
        }

        /// <summary>
        /// binding command
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <param name="commandAction"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static RelayCommandAsync<Target> Bind<Target>(Func<Target, Task> commandAction, Action<Exception> exceptionCallback)
        {
            return new RelayCommandAsync<Target>(commandAction, (t) => true, exceptionCallback);
        }
    }
}