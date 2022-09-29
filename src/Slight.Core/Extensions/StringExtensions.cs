using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slight.Core
{
    /// <summary>
    /// simple string  extension
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Compare two strings for equality
        /// </summary>
        /// <param name="originString">string</param>
        /// <param name="childString">other string</param>
        /// <param name="stringComparison"><see cref="StringComparison"/></param>
        /// <returns></returns>
        public static bool Contains(this string originString, string childString, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            if (originString is null || childString is null)
            {
                return false;
            }
            bool result = originString.IndexOf(childString, stringComparison) >= 0;

            return result;
        }


        /// <summary>
        /// Compare two strings for equality
        /// </summary>
        /// <param name="originString">string</param>
        /// <param name="compareString">other string</param>
        /// <param name="stringComparison"><see cref="StringComparison"/></param>
        /// <returns></returns>
        public static bool Compare(this string originString, string compareString, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return originString is null || compareString is null ? false : string.Compare(originString, compareString, stringComparison) == 0;
        }

        /// <summary>
        ///  Join a <typeparamref name="TTarget"/> collection
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="targets">collection</param>
        /// <param name="joinString">join char</param>
        /// <param name="selector">value selecor</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Join<TTarget>(this IEnumerable<TTarget> targets, string joinString, Func<TTarget, string> selector)
        {
            if (targets is null)
            {
                throw new ArgumentNullException(nameof(targets));
            }

            return joinString is null
                ? throw new ArgumentNullException(nameof(joinString))
                : selector is null
                ? throw new ArgumentNullException(nameof(selector))
                : string.Join(joinString, targets.Select(i => selector(i)));
        }

        /// <summary>
        ///  Join a  <typeparamref name="TTarget"/> collection
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="targets">collection</param>
        /// <param name="joinString">join char</param> 
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Join<TTarget>(this IEnumerable<TTarget> targets, string joinString = ",")
        {
            return targets is null
                ? throw new ArgumentNullException(nameof(targets))
                : joinString is null ? throw new ArgumentNullException(nameof(joinString)) : string.Join(joinString, targets);
        }

        /// <summary>
        /// create <see cref="System.Text.StringBuilder"/> from <typeparamref name="Target"/> Collection
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <param name="collection">collection</param>
        /// <param name="action">builder function</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static StringBuilder StringBuilder<Target>(this IEnumerable<Target> collection, Action<StringBuilder, Target> action)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            StringBuilder stringBuilder = new();

            foreach (Target item in collection)
            {
                action(stringBuilder, item);
            }

            return stringBuilder;
        }
    }
}