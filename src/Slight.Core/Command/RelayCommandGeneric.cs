using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Slight.Core
{
    /// <summary>
    ///  a class of <see cref="RelayCommand{TParameter}"/>
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public sealed class RelayCommand<TParameter> : ICommand<TParameter>, IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private bool isExecuting;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Collection<Action<TParameter>> executeActions = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Func<TParameter, bool> canExecuteFunc;
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
        /// create a new instance
        /// </summary>
        /// <param name="executeAction">command body</param>
        /// <param name="canExecuteFunc">can execute fnction</param>
        /// <param name="exceptionCallback">exception callbase</param>
        public RelayCommand(Action<TParameter> executeAction, Func<TParameter, bool> canExecuteFunc = null, Action<Exception> exceptionCallback = null)
        {
            if (executeAction is null)
            {
                throw new ArgumentNullException(nameof(executeAction));
            }
            executeActions?.Add(executeAction);
            this.canExecuteFunc = canExecuteFunc;
            this.exceptionCallback = exceptionCallback;
        }



        /// <summary>
        /// Append an other command into the Command body
        /// </summary>
        /// <param name="executableCommandCallback">an other command body</param>
        /// <param name="cammandName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand<TParameter> Append(Action<TParameter> executableCommandCallback, string cammandName = null)
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

            return CanExecute(target);
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
        void ICommand.Execute(object parameter)
        {
            if (parameter is TParameter target)
            {
                Execute(target);
            }
        }

        /// <summary>
        ///  Execute Command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(TParameter parameter)
        {

            lock (executeActions)
            {
                if (executeActions is null || executeActions.Count == 0 || isExecuting || CanExecute(parameter) == false)
                {
                    return;
                }

                isExecuting = true;
            }
            try
            {
                executeActions?.ForEach(i => i?.Invoke(parameter));
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
    }
}