using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Slight.Core
{
    /// <summary>
    /// simple invoke class
    /// </summary>
    public static class Invoker
    {
        public static void WhenNotNull<Target>(Target target, Action<Target> whenCallback) where Target : class
        {
            if (target is null)
            {
                return;
            }

            whenCallback?.Invoke(target);
        }

        public static void WhenTrue(bool condition, Action whenCallback)
        {
            if (condition)
            {
                whenCallback?.Invoke();
            }
        }

        public static void WhenTrue(Func<bool> condition, Action whenCallback)
        {
            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (condition.Invoke() == false)
            {
                return;
            }

            whenCallback?.Invoke();
        }

        /// <summary>
        /// run delegate and ignore exception
        /// </summary>
        /// <param name="action">run body</param>
        /// <param name="exceptionCallback">exception callback</param>
        public static void TryRun(Action action, Action<Exception> exceptionCallback = null)
        {
            if (action is null)
            {
                return;
            }

            try
            {
                action();
            }
            catch (Exception exception)
            {
                exceptionCallback?.Invoke(exception);
            }
        }

        /// <summary>
        /// loop 
        /// </summary>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count [ ? > 0]</param>
        /// <param name="loopBody">loopBody</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void For(int startIndex, int count, Action<int> loopBody)
        {
            if (loopBody is null)
            {
                throw new ArgumentNullException(nameof(loopBody));
            }

            if (count <= 0)
            {
                return;
            }

            for (int i = startIndex, j = startIndex + count; i < j; i++)
            {
                loopBody(i);
            }
        }

        /// <summary>
        /// loop 
        /// </summary>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count [ ? > 0]</param>
        /// <param name="loopBody">loopBody</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void For(int startIndex, int count, Action loopBody)
        {
            if (loopBody is null)
            {
                throw new ArgumentNullException(nameof(loopBody));
            }

            if (count <= 0)
            {
                return;
            }

            for (int i = startIndex, j = startIndex + count; i < j; i++)
            {
                loopBody();
            }
        }

        /// <summary>
        /// loop async
        /// </summary>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count [ ? > 0]</param>
        /// <param name="loopBody">loopBody</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task ForAsync(int startIndex, int count, Action<int, CancellationToken> loopBody, CancellationToken token = default)
        {

            if (loopBody is null)
            {
                throw new ArgumentNullException(nameof(loopBody));
            }

            if (count <= 0)
            {
                return Task.FromResult(false);
            }

            Task targetTask = Task.Factory.StartNew(() =>
            {
                for (int i = startIndex, j = startIndex + count; i < j; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    loopBody(i, token);
                }
            }, token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

            return targetTask;
        }

        /// <summary>
        /// loop async
        /// </summary>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count [ ? > 0]</param>
        /// <param name="loopBody">loopBody</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task ForAsync(int startIndex, int count, Action<CancellationToken> loopBody, CancellationToken token = default)
        {
            return loopBody is null
                ? throw new ArgumentNullException(nameof(loopBody))
                : count <= 0
                ? Task.FromResult(false)
                : Task.Factory.StartNew(() =>
                {
                    for (int i = startIndex, j = startIndex + count; i < j; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }
                        loopBody(token);
                    }
                }, token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);


        }

        /// <summary>
        /// loop async
        /// </summary>
        /// <param name="loopCondition">condition</param>
        /// <param name="loopBody">loopBody</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task WhileAsync(Func<bool> loopCondition, Action<CancellationToken> loopBody, CancellationToken token = default)
        {
            return loopCondition is null
                ? throw new ArgumentNullException(nameof(loopCondition))
                : loopBody is null
                ? throw new ArgumentNullException(nameof(loopBody))
                : Task.Factory.StartNew(() =>
                {
                    while (loopCondition.Invoke() && !token.IsCancellationRequested)
                    {
                        loopBody(token);
                    }
                }, token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        /// <summary>
        /// loop  
        /// </summary>
        /// <param name="loopCondition">condition</param>
        /// <param name="loopBody">loopBody</param> 
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static void While(Func<bool> loopCondition, Action loopBody)
        {
            if (loopCondition is null)
            {
                throw new ArgumentNullException(nameof(loopCondition));
            }
            if (loopBody is null)
            {
                throw new ArgumentNullException(nameof(loopBody));
            }

            while (loopCondition.Invoke())
            {
                loopBody();
            }
        }

        /// <summary>
        /// run delegate async
        /// </summary>
        /// <param name="action">delegate body</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <param name="creationOptions"><see cref="TaskContinuationOptions"/></param>
        /// <returns></returns>
        public static Task RunAsync(Action action, CancellationToken token = default, TaskCreationOptions creationOptions = TaskCreationOptions.DenyChildAttach)
        {
            return action is null
                ? Task.FromResult(false)
                : Task.Factory.StartNew(action, token, creationOptions, TaskScheduler.Default);
        }

        /// <summary>
        /// run delegate async
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action">delegate body</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <param name="creationOptions"><see cref="TaskCreationOptions"/></param>
        /// <returns></returns>
        public static Task<TResult> RunAsync<TResult>(Func<TResult> action, CancellationToken token = default, TaskCreationOptions creationOptions = TaskCreationOptions.DenyChildAttach)
        {
            return action is null
                ? Task.FromResult<TResult>(default)
                : Task.Factory.StartNew(action, token, creationOptions, TaskScheduler.Default);
        }

        /// <summary>
        /// If the <see cref="IDisposable"/> is inherited, execute
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object TryDispose(object obj)
        {

            if (obj is IEnumerable and)
            {
                foreach (IDisposable item in and.OfType<IDisposable>())
                {
                    item?.Dispose();
                }
                return obj;
            }

            if (obj is IDisposable and1)
            {
                and1?.Dispose();
            }

            return obj;
        }

        /// <summary>
        /// cast object value to target Type
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <param name="value">object value</param>
        /// <param name="outValue">target value</param>
        /// <returns>cast success</returns>
        public static bool TryCast<Target>(object value, out Target outValue)
        {
            if (value != null)
            {
                try
                {
                    if (value is Target target)
                    {
                        outValue = target;
                        return true;
                    }
                    outValue = (Target)Convert.ChangeType(value, typeof(Target));
                    return true;
                }
                catch
                {
                }
            }
            outValue = default;
            return false;
        }

        /// <summary>
        /// cast object value to target Type
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <param name="value">object value</param> 
        /// <returns>cast success</returns>
        public static Target CastTo<Target>(object value)
        {
            if (value is null)
            {
                return default;
            }

            try
            {
                return value is Target target ? target : (Target)Convert.ChangeType(value, typeof(Target));
            }
            catch
            {
                return default;
            }
        }
    }
}
