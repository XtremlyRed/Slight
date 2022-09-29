using System.Diagnostics;
using System.Linq.Expressions;

namespace Slight.Core
{

    public static class PropertyAssist
    {
        #region  binding

        public static BindableProperty PropertyRegister<TBindableObject, TPropertyType>(Expression<Func<TBindableObject, TPropertyType>> propertyNameSelector, Action<TBindableObject, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null) where TBindableObject : BindableObject
        {
            string propertyName = Ref.GetPropertyName(propertyNameSelector);
            BindableProperty property = BindableProperty.Create(propertyName, typeof(TPropertyType), typeof(TBindableObject), default(TPropertyType), BindingMode.TwoWay, null, (sender, old, @new) =>
            {
                TBindableObject @object = sender as TBindableObject;
                PropertyChangedEventArgs<TPropertyType> args = new(@object, @new, old);

                propertyChangedCallback?.Invoke(@object, args);

            });
            return property;
        }

        public static BindableProperty PropertyRegister<TBindableObject, TPropertyType>(Expression<Func<TBindableObject, TPropertyType>> propertyNameSelector, TPropertyType defaultValue = default, Action<TBindableObject, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null) where TBindableObject : BindableObject
        {
            string propertyName = Ref.GetPropertyName(propertyNameSelector);
            BindableProperty property = BindableProperty.Create(propertyName, typeof(TPropertyType), typeof(TBindableObject), defaultValue, BindingMode.TwoWay, null, (sender, old, @new) =>
            {
                TBindableObject @object = sender as TBindableObject;
                PropertyChangedEventArgs<TPropertyType> args = new(@object, @new, old);

                propertyChangedCallback?.Invoke(@object, args);

            });
            return property;
        }

        public static BindableProperty PropertyRegister<TBindableObject, TPropertyType>(Expression<Func<TBindableObject, TPropertyType>> propertyNameSelector, TPropertyType defaultValue = default, BindingMode defaultBindingMode = BindingMode.OneWay, Action<TBindableObject, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null) where TBindableObject : BindableObject
        {
            string propertyName = Ref.GetPropertyName(propertyNameSelector);
            BindableProperty property = BindableProperty.Create(propertyName, typeof(TPropertyType), typeof(TBindableObject), defaultValue, defaultBindingMode, null, (sender, old, @new) =>
            {
                TBindableObject @object = sender as TBindableObject;
                PropertyChangedEventArgs<TPropertyType> args = new(@object, @new, old);

                propertyChangedCallback?.Invoke(@object, args);

            });
            return property;
        }

        #endregion

        #region

        #endregion


        [DebuggerDisplay("property:{Property.Name}  new value:{NewValue}  old value:{OldValue}")]
        public class PropertyChangedEventArgs<TargetType> : EventArgs
        {
            internal PropertyChangedEventArgs(BindableObject @object, object newValue, object oldValue)
            {
                Object = @object;
                OldValue = Invoker.CastTo<TargetType>(oldValue);
                NewValue = Invoker.CastTo<TargetType>(newValue);
            }

            public BindableObject Object { get; }
            public TargetType OldValue { get; }
            public TargetType NewValue { get; }
        }
    }
}
