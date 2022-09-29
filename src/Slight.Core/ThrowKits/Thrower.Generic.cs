using System;
using System.Runtime.CompilerServices;

namespace Slight.Core
{
    public static partial class Thrower
    {
        /// <summary>
        /// throw <typeparamref name="TException"/> if true
        /// </summary>
        /// <typeparam name="TException">target exrption  [Must have construction:[<typeparamref name="TException"/> (string message)]  ]</typeparam>
        /// <param name="predicate">condition check body</param>
        /// <param name="exceptionMessage">exception Message</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void IfTrue<TException>(Func<bool> predicate, string exceptionMessage, bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default)
            where TException : Exception
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                throw new ArgumentNullException(nameof(exceptionMessage));
            }

            if (!predicate.Invoke())
            {
                return;
            }

            string throwExceptionMessage = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);
            throw (TException)Activator.CreateInstance(typeof(TException), $"{exceptionMessage}{throwExceptionMessage}");
        }

        /// <summary>
        /// throw <typeparamref name="TException"/> if false
        /// </summary>
        /// <typeparam name="TException">target exrption  [Must have construction:[<typeparamref name="TException"/> (string message)]  ]</typeparam>
        /// <param name="predicate">condition check body</param>
        /// <param name="exceptionMessage">exception Message</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void IfFalse<TException>(Func<bool> predicate, string exceptionMessage, bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default)
            where TException : Exception
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                throw new ArgumentNullException(nameof(exceptionMessage));
            }

            if (predicate.Invoke())
            {
                return;
            }

            string throwExceptionMessage = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);
            throw (TException)Activator.CreateInstance(typeof(TException), $"{exceptionMessage}{throwExceptionMessage}");
        }

        /// <summary>
        /// throw <typeparamref name="TException"/>  if true
        /// </summary>
        /// <typeparam name="TException">target exrption  [Must have construction:[<typeparamref name="TException"/> (string message)]  ]</typeparam>
        /// <param name="condition">condition   body</param>
        /// <param name="exceptionMessage">exception Message</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void IfTrue<TException>(bool condition, string exceptionMessage, bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default)
            where TException : Exception
        {
            if (!condition)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                throw new ArgumentNullException(nameof(exceptionMessage));
            }

            string throwExceptionMessage = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);
            throw (TException)Activator.CreateInstance(typeof(TException), $"{exceptionMessage}{throwExceptionMessage}");
        }
        /// <summary>
        /// throw <typeparamref name="TException"/>  if false
        /// </summary>
        /// <typeparam name="TException">target exrption  [Must have construction:[<typeparamref name="TException"/> (string message)]  ]</typeparam>
        /// <param name="condition">condition   body</param>
        /// <param name="exceptionMessage">exception Message</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void IfFalse<TException>(bool condition, string exceptionMessage, bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default)
            where TException : Exception
        {
            if (condition)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                throw new ArgumentNullException(nameof(exceptionMessage));
            }

            string throwExceptionMessage = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);
            throw (TException)Activator.CreateInstance(typeof(TException), $"{exceptionMessage}{throwExceptionMessage}");
        }


        /// <summary>
        /// throw <typeparamref name="TException"/>  
        /// </summary>
        /// <typeparam name="TException">target exrption  [Must have construction:[<typeparamref name="TException"/> (string message)]  ]</typeparam>
        /// <param name="exception">exception</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        public static void Throw<TException>(this TException exception, bool displayFilePath = false,
           [CallerFilePath] string callerFilePath = null,
           [CallerLineNumber] int callerLineNumber = default)
           where TException : Exception
        {
            if (exception is null)
            {
                return;
            }

            string throwExceptionMessage = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);


            TException exp = (TException)Activator.CreateInstance(typeof(TException), throwExceptionMessage, exception);

            throw exp;
        }
    }
}