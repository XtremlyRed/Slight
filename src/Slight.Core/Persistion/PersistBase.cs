using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection; 
using bf = System.Reflection.BindingFlags;
namespace Slight.Core
{

    /// <summary>
    ///  PersistBase
    /// </summary>
    public abstract class PersistBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static readonly IDictionary<Type, MapperInfo> mapper = new ConcurrentDictionary<Type, MapperInfo>();
        protected abstract IDictionary<string, byte[]> Read();

        protected abstract void Wtire(IReadOnlyDictionary<string, byte[]> persistMapper);


        /// <summary>
        /// Store the current object data
        /// </summary> 
        public virtual void Persist()
        {
            MapperInfo mapperInfo = GetMapperInfos();

            Dictionary<string, byte[]> dict = new();

            foreach (PropertyInfo item in mapperInfo.PropertyInfos)
            {
                object obj = item.GetValue(this);

                byte[] targetStringValue = mapperInfo.PersistConvert.Convert(obj);

                dict[item.Name] = targetStringValue;
            }

            Wtire(dict);
        }


        /// <summary>
        /// Map stored data to the current object
        /// </summary>
        public virtual void Mapper()
        {
            MapperInfo mapperInfo = GetMapperInfos();

            IDictionary<string, byte[]> readMapper = Read();

            foreach (PropertyInfo item in mapperInfo.PropertyInfos)
            {
                if (readMapper.TryGetValue(item.Name, out byte[] value))
                {
                    object targetValue = mapperInfo.PersistConvertBack.ConvertBack(value, item.PropertyType);
                    item.SetValue(this, targetValue);

                    readMapper.Remove(item.Name);
                }
            }
        }

        [DebuggerNonUserCode]
        private MapperInfo GetMapperInfos()
        {
            Type currentType = GetType();

            if (mapper.TryGetValue(currentType, out MapperInfo value) == false)
            {
                PropertyInfo[] properties = currentType
                    .GetProperties(bf.Instance | bf.Public)
                    .Where(i => i.CanWrite && i.CanRead)
                    .ToDictionary(i => i, i => i.GetAttribute<PersistIgnoreAttribute>())
                    .Where(i => i.Value is null)
                    .Select(i => i.Key)
                    .ToArray();

                mapper[currentType] = value = new MapperInfo();

                PersistConvertAttribute attr = currentType.GetAttribute<PersistConvertAttribute>();

                Type backConverterType = typeof(IPersistConvertBack);

                Type converterType = typeof(IPersistConvert);

                if (attr is null)
                {
                    throw new ArgumentException($"{currentType} must be added Attribute:{typeof(PersistConvertAttribute)}");
                }
                if (converterType.IsAssignableFrom(attr.ConvertType) == false)
                {
                    throw new ArgumentException($"{converterType} must be assignable from {nameof(PersistConvertAttribute)}.{nameof(attr.ConvertType)}");
                }
                if (backConverterType.IsAssignableFrom(attr.ConvertBackType) == false)
                {
                    throw new ArgumentException($"{backConverterType} must be assignable from{nameof(PersistConvertAttribute)}.{nameof(attr.ConvertBackType)}");
                }

                value.PropertyInfos = properties;
                value.PersistConvert = Activator.CreateInstance(attr.ConvertType) as IPersistConvert;
                value.PersistConvertBack = Activator.CreateInstance(attr.ConvertBackType) as IPersistConvertBack;
            }

            return value;
        }


        [DebuggerNonUserCode]
        private class MapperInfo
        {
            public PropertyInfo[] PropertyInfos { get; set; }
            public IPersistConvert PersistConvert { get; set; }
            public IPersistConvertBack PersistConvertBack { get; set; }
        }
    }

    [DebuggerNonUserCode]
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class PersistConvertAttribute : Attribute
    {
        public PersistConvertAttribute(Type convertType, Type convertBackType)
        {
            ConvertType = convertType;
            ConvertBackType = convertBackType;
        }
        public Type ConvertType { get; }
        public Type ConvertBackType { get; }
    }

    public interface IPersistConvert
    {
        byte[] Convert(object propertyValue);
    }


    public interface IPersistConvertBack
    {
        object ConvertBack(byte[] targetPropertyStringValue, Type targetPropertyType);
    }
}
