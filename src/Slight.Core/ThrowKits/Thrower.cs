
using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Slight.Core
{
    /// <summary>
    /// simple exception thrower
    /// </summary>
    public static partial class Thrower
    {

        /// <summary>
        /// message format:"on the line:{0} in file:{1}"
        /// </summary>
        public static string MessageFormater = "on the line:{0} of file:{1}";

        /// <summary>
        /// throw exception if null
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="target">target object</param>
        /// <param name="nullMessage">nullMessage</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="NullReferenceException"></exception>
        public static TObject ThrowIfNull<TObject>(this TObject target, string nullMessage = null, bool displayFilePath = false,
           [CallerFilePath] string callerFilePath = null,
           [CallerLineNumber] int callerLineNumber = default) where TObject : class
        {
            if (target != null)
            {
                return target;
            }

            string message = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);

            string msg = nullMessage ?? "object is null";

            throw new NullReferenceException($"{msg}{Environment.NewLine}{message}");
        }

        /// <summary>
        /// throw exception if null
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="target">target object</param>
        /// <param name="nullMessage">target object name</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static TObject IfNull<TObject>(TObject target, string nullMessage,
            bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default) where TObject : class
        {
            if (target != null)
            {
                return target;
            }

            if (string.IsNullOrWhiteSpace(nullMessage))
            {
                throw new ArgumentNullException(nameof(nullMessage));
            }


            string message = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);



            throw new ArgumentException($"{nullMessage}{message}");
        }

        /// <summary>
        /// throw exception if null
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="targetExpression">target object selector</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static TObject IfNull<TObject>(Expression<Func<TObject>> targetExpression,
            bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default) where TObject : class
        {
            if (targetExpression is null)
            {
                throw new ArgumentNullException(nameof(targetExpression));
            }
            TObject rarget = targetExpression.Compile().Invoke();
            if (rarget != null)
            {
                return rarget;
            }

            string targetName = targetExpression.GetMemberName();

            string message = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);



            throw new ArgumentNullException(targetName, $"{targetName} is Null{message}");
        }

        /// <summary>
        /// throw exception if true
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="targetExpression">object selector body</param>
        /// <param name="checkFunc">condition check body</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public static TObject IfTrue<TObject>(Expression<Func<TObject>> targetExpression,
              Func<TObject, bool> checkFunc, bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default)
        {
            if (targetExpression is null)
            {
                throw new ArgumentNullException(nameof(targetExpression));
            }

            if (checkFunc is null)
            {
                throw new ArgumentNullException(nameof(checkFunc));
            }

            TObject target = targetExpression.Compile().Invoke();
            string targetName = targetExpression.GetMemberName();

            if (!checkFunc.Invoke(target))
            {
                return target;
            }

            string message = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);



            throw new Exception($"{targetName} is Error{message}");
        }

        /// <summary>
        /// throw exception if false
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="targetExpression">object selector body</param>
        /// <param name="checkFunc">condition check body</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public static TObject IfFalse<TObject>(Expression<Func<TObject>> targetExpression,
              Func<TObject, bool> checkFunc, bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default)
        {
            if (targetExpression is null)
            {
                throw new ArgumentNullException(nameof(targetExpression));
            }

            if (checkFunc is null)
            {
                throw new ArgumentNullException(nameof(checkFunc));
            }

            TObject target = targetExpression.Compile().Invoke();
            string targetName = targetExpression.GetMemberName();

            if (checkFunc.Invoke(target))
            {
                return target;
            }

            string message = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);



            throw new Exception($"{targetName} is Error{message}");
        }


        /// <summary>
        /// throw exception if true
        /// </summary>
        /// <param name="predicate">condition check body</param>
        /// <param name="exceptionMessage">exception Message</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public static void IfTrue(Func<bool> predicate, string exceptionMessage, bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default)
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

            string message1 = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);



            throw new Exception($"{exceptionMessage}{message1}");
        }



        /// <summary>
        /// throw exception if false
        /// </summary>
        /// <param name="predicate">condition check body</param>
        /// <param name="exceptionMessage">exception Message</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public static void IfFalse(Func<bool> predicate, string exceptionMessage, bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default)
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

            string message1 = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);



            throw new Exception($"{exceptionMessage}{message1}");
        }


        /// <summary>
        /// throw exception if true
        /// </summary>
        /// <param name="condition">condition   body</param>
        /// <param name="exceptionMessage">exception Message</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void IfTrue(bool condition, string exceptionMessage, bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default)
        {
            if (!condition)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                throw new ArgumentNullException(nameof(exceptionMessage));
            }

            string message1 = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);



            throw new ArgumentException($"{exceptionMessage}{message1}");
        }

        /// <summary>
        /// throw exception if true
        /// </summary>
        /// <param name="condition">condition   body</param>
        /// <param name="exceptionMessage">exception Message</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void IfFalse(bool condition, string exceptionMessage, bool displayFilePath = false,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = default)
        {
            if (condition)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                throw new ArgumentNullException(nameof(exceptionMessage));
            }


            string message1 = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);



            throw new ArgumentException($"{exceptionMessage}{message1}");
        }


        /// <summary>
        /// Throw <see cref="Exception"/> by exceptionMessage
        /// </summary>
        /// <param name="exceptionMessage">exception Message</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Throw(string exceptionMessage, bool displayFilePath = false,
       [CallerFilePath] string callerFilePath = null,
       [CallerLineNumber] int callerLineNumber = default)
        {
            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                throw new ArgumentNullException(nameof(exceptionMessage));
            }

            string throwExceptionMessage = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);

            Exception exp = new($"{exceptionMessage}{throwExceptionMessage}");

            throw exp;
        }



        /// <summary>
        /// Throw <see cref="Exception"/> by exceptionMessage
        /// </summary>
        /// <param name="exceptionMessage">exception Message</param>
        /// <param name="displayFilePath">append code file Path into exceition message</param>
        /// <param name="callerFilePath">must be keep default value</param>
        /// <param name="callerLineNumber">must be keep default value</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static Target Throw<Target>(string exceptionMessage, bool displayFilePath = false,
       [CallerFilePath] string callerFilePath = null,
       [CallerLineNumber] int callerLineNumber = default)
        {
            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                throw new ArgumentNullException(nameof(exceptionMessage));
            }

            string throwExceptionMessage = FormatMessage(callerFilePath, callerLineNumber, displayFilePath);

            Exception exp = new($"{exceptionMessage}{throwExceptionMessage}");

            throw exp;
        }



        private static string FormatMessage(string callerFilePath, int callerLineNumber, bool displayFilePath = false)
        {

            string path = displayFilePath ? callerFilePath : Path.GetFileName(callerFilePath);

            try
            {
                return $"{Environment.NewLine}{string.Format(MessageFormater, callerLineNumber, path)}";
            }
            catch
            {
                const string message = "{0}on the line:{1} of file:{2}";
                return string.Format(message, Environment.NewLine, callerLineNumber, path);
            }
        }
    }
}