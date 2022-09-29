using System.Linq.Expressions;
using System.Reflection;
namespace Slight.Core
{
    public static class BindingAssist
    {
        public static void Binding<TBindableObject, TSource>(this
            TBindableObject bindableObject,
            Expression<Func<TBindableObject, object>> bindableObjectPropertyNameExpression,
            TSource source,
            Expression<Func<TSource, object>> propertyNameExpression,
            BindingMode bindingMode = BindingMode.Default)
            where TSource : class
            where TBindableObject : BindableObject
        {
            string bindableObjectPropertyName = $"{Ref.GetPropertyName(bindableObjectPropertyNameExpression)}Property";
            Type type = bindableObject.GetType();
            FieldInfo fieldInfo = null;

            do
            {
                fieldInfo = type.GetField(bindableObjectPropertyName);
                type = type.BaseType;
            } while (fieldInfo is null);

            BindableProperty targetProperty = fieldInfo.GetValue(bindableObject) as BindableProperty;
            string propertyName = Ref.GetPropertyName(propertyNameExpression);
            bindableObject.SetBinding(targetProperty, new Binding(propertyName, bindingMode, null, null, null, source));
        }

        public static void SetBinding<TSource, TConverter>(this
           BindableObject bindableObject,
           BindableProperty targetProperty,
           TSource source,
           Expression<Func<TSource, object>> propertyExpression,
           BindingMode bindingMode = BindingMode.Default,
           TConverter converter = default)
           where TSource : class
            where TConverter : IValueConverter

        {
            string propertyName = Ref.GetPropertyName(propertyExpression);
            bindableObject.SetBinding(targetProperty, new Binding(propertyName, bindingMode, converter, null, null, source));
        }
    }
}
