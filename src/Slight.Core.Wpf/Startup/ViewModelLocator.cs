using System;
using System.Globalization;
using System.Reflection;
using System.Windows;

namespace Slight.Core
{

    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    //public class ViewModelLocatorAttribute : Attribute
    //{
    //    public ViewModelLocatorAttribute(Type viewModelType)
    //    {
    //        ViewModelType = viewModelType;
    //    }

    //    public Type ViewModelType { get; }
    //}


    public class ViewModelLocator : DependencyObject
    {
        public static bool? GetAutoAware(DependencyObject obj)
        {
            return (bool?)obj.GetValue(AutoAwareProperty);
        }

        public static void SetAutoAware(DependencyObject obj, bool? value)
        {
            obj.SetValue(AutoAwareProperty, value);
        }


        public static readonly DependencyProperty AutoAwareProperty =
            DependencyProperty.RegisterAttached("AutoAware", typeof(bool?),
            typeof(ViewModelLocator),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (s, e) =>
            {
                if (e.NewValue is not true)
                {
                    return;
                }

                if (s is not FrameworkElement element)
                {
                    return;
                }
                Type viewType = s.GetType();

                Type viewModelType = viewModelType = defaultViewTypeToViewModelTypeResolver(viewType);

                //      viewType.GetAttribute<ViewModelLocatorAttribute>()?.ViewModelType;
                 
                if (viewModelType is null)
                {
                    return;
                }

                object viewModel = SlightApplication.Provider.Resolve(viewModelType);

                if (viewModel is null)
                {
                    return;
                }

                element.DataContext = viewModel;

            }));


        private static Func<Type, Type> defaultViewTypeToViewModelTypeResolver =
          viewType =>
          {
              string viewName = viewType.FullName;
              viewName = viewName.Replace(".Views.", ".ViewModels.");
              string viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
              string suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
              string viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
              return Type.GetType(viewModelName);
          };


        public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
        {
            defaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
        }
    }
}
