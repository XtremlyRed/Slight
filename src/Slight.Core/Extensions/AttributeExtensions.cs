
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Slight.Core
{
    /// <summary>
    /// Attribute Extensions
    /// </summary>
    public static partial class AttributeExtensions
    {
        /// <summary>
        /// get <typeparamref name="TAttribute"/> from <see cref="Type"/>
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            object obj = type.GetCustomAttributes(true).FirstOrDefault(i => i is TAttribute);

            return obj as TAttribute;
        }

        /// <summary>
        /// get  <typeparamref name="TAttribute"/>  from <see cref="PropertyInfo"/>
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TAttribute GetAttribute<TAttribute>(this PropertyInfo property) where TAttribute : Attribute
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            object obj = property.GetCustomAttributes(true).FirstOrDefault(i => i is TAttribute);

            return obj as TAttribute;
        }

        /// <summary>
        ///  get  <typeparamref name="TAttribute"/>  from <see cref="FieldInfo"/>
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TAttribute GetAttribute<TAttribute>(this FieldInfo field) where TAttribute : Attribute
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            object obj = field.GetCustomAttributes(true).FirstOrDefault(i => i is TAttribute);

            return obj as TAttribute;
        }

        /// <summary>
        /// get  <typeparamref name="TAttribute"/>  from <see cref="MemberInfo"/>
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo field) where TAttribute : Attribute
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            object obj = field.GetCustomAttributes(true).FirstOrDefault(i => i is TAttribute);

            return obj as TAttribute;
        }


        /// <summary>
        /// get all attributes from <see cref="Type"/>
        /// </summary>
        /// <param name="type"><see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICollection<Attribute> GetAttributes(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            Attribute[] attrs = type.GetCustomAttributes(true).OfType<Attribute>().ToArray();

            return attrs;
        }

        /// <summary>
        /// get all attributes from <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="property"><see cref="PropertyInfo"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICollection<Attribute> GetAttributes(this PropertyInfo property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            Attribute[] attrs = property.GetCustomAttributes(true).OfType<Attribute>().ToArray();

            return attrs;
        }
        /// <summary>
        /// get all attributes from <see cref="FieldInfo"/>
        /// </summary>
        /// <param name="field"><see cref="FieldInfo"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICollection<Attribute> GetAttributes(this FieldInfo field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            Attribute[] attrs = field.GetCustomAttributes(true).OfType<Attribute>().ToArray();

            return attrs;
        }
        /// <summary>
        /// get all attributes from <see cref="MemberInfo"/>
        /// </summary>
        /// <param name="member"><see cref="MemberInfo"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICollection<Attribute> GetAttributes(this MemberInfo member)
        {
            if (member is null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            Attribute[] attrs = member.GetCustomAttributes(true).OfType<Attribute>().ToArray();

            return attrs;
        }




        #region ENUM

        private static readonly ConcurrentDictionary<Type, IDictionary<Enum, Attribute[]>> enumAttributeDictionary = new();


        /// <summary>
        ///   get  <typeparamref name="TAttribute"/>  from <see cref="Enum"/>
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="enumValue"><see cref="Enum"/></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            if (enumValue == null)
            {
                return default;
            }

            Type type = enumValue.GetType();

            if (!enumAttributeDictionary.TryGetValue(type, out IDictionary<Enum, Attribute[]> dicts))
            {
                enumAttributeDictionary[type] = dicts = new Dictionary<Enum, Attribute[]>();

                List<FieldInfo> list = enumValue.GetType().GetFields().Where(i => i.IsStatic && !i.IsSpecialName).ToList();

                foreach (FieldInfo fieldInfo in list)
                {
                    if (fieldInfo.GetValue(null) is Enum @enum)
                    {
                        dicts[@enum] = fieldInfo.GetCustomAttributes(false).OfType<Attribute>().ToArray();
                    }
                }
            }

            return !dicts.TryGetValue(enumValue, out Attribute[] atts) ? default : atts.OfType<TAttribute>().FirstOrDefault();
        }



        #endregion



    }
}