using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;

namespace Slight.Core
{


    public static class PropertyAssist
    {
        #region  binding

        public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector, Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null) where TDependencyObject : DependencyObject
        {
            string propertyName = Ref.GetPropertyName(propertyNameSelector);
            DependencyProperty property = DependencyProperty.Register(propertyName, typeof(TPropertyType), typeof(TDependencyObject), new PropertyMetadata(default(TPropertyType), (s, e) =>
            {
                if (propertyChangedCallback is null)
                {
                    return;
                }

                PropertyChangedEventArgs<TPropertyType> args = new(e.Property, e.NewValue, e.OldValue);
                TDependencyObject sender = s as TDependencyObject;
                propertyChangedCallback.Invoke(sender, args);

            }));
            return property;
        }

        public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector, TPropertyType defaultValue, Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null) where TDependencyObject : DependencyObject
        {
            string propertyName = Ref.GetPropertyName(propertyNameSelector);
            DependencyProperty property = DependencyProperty.Register(propertyName, typeof(TPropertyType), typeof(TDependencyObject), new PropertyMetadata(defaultValue, (s, e) =>
            {
                if (propertyChangedCallback is null)
                {
                    return;
                }

                PropertyChangedEventArgs<TPropertyType> args = new(e.Property, e.NewValue, e.OldValue);
                TDependencyObject sender = s as TDependencyObject;
                propertyChangedCallback.Invoke(sender, args);

            }));
            return property;
        }


        public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector, TPropertyType defaultValue, UpdateSourceTrigger updateSourceTrigger, Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null) where TDependencyObject : DependencyObject
        {
            string propertyName = Ref.GetPropertyName(propertyNameSelector);
            FrameworkPropertyMetadataOptions flags = FrameworkPropertyMetadataOptions.BindsTwoWayByDefault;
            DependencyProperty property = DependencyProperty.Register(propertyName, typeof(TPropertyType), typeof(TDependencyObject), new FrameworkPropertyMetadata(defaultValue, flags, (s, e) =>
            {
                if (propertyChangedCallback is null)
                {
                    return;
                }

                PropertyChangedEventArgs<TPropertyType> args = new(e.Property, e.NewValue, e.OldValue);
                TDependencyObject sender = s as TDependencyObject;
                propertyChangedCallback.Invoke(sender, args);

            }, null, false, updateSourceTrigger));
            return property;
        }

        public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector, TPropertyType defaultValue, FrameworkPropertyMetadataOptions flags, UpdateSourceTrigger updateSourceTrigger, Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null) where TDependencyObject : DependencyObject
        {
            string propertyName = Ref.GetPropertyName(propertyNameSelector);
            DependencyProperty property = DependencyProperty.Register(propertyName, typeof(TPropertyType), typeof(TDependencyObject), new FrameworkPropertyMetadata(defaultValue, flags, (s, e) =>
            {
                if (propertyChangedCallback is null)
                {
                    return;
                }

                PropertyChangedEventArgs<TPropertyType> args = new(e.Property, e.NewValue, e.OldValue);
                TDependencyObject sender = s as TDependencyObject;
                propertyChangedCallback.Invoke(sender, args);

            }, null, false, updateSourceTrigger));
            return property;
        }

        public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector, TPropertyType defaultValue, FrameworkPropertyMetadataOptions flags, Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null) where TDependencyObject : DependencyObject
        {
            string propertyName = Ref.GetPropertyName(propertyNameSelector);

            DependencyProperty property = DependencyProperty.Register(propertyName, typeof(TPropertyType), typeof(TDependencyObject), new FrameworkPropertyMetadata(defaultValue, flags, (s, e) =>
            {
                if (propertyChangedCallback is null)
                {
                    return;
                }

                PropertyChangedEventArgs<TPropertyType> args = new(e.Property, e.NewValue, e.OldValue);
                TDependencyObject sender = s as TDependencyObject;
                propertyChangedCallback.Invoke(sender, args);

            }));
            return property;
        }


        public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector, FrameworkPropertyMetadataOptions flags, Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null) where TDependencyObject : DependencyObject
        {
            string propertyName = Ref.GetPropertyName(propertyNameSelector);
            TPropertyType defaultValue = default;
            DependencyProperty property = DependencyProperty.Register(propertyName, typeof(TPropertyType), typeof(TDependencyObject), new FrameworkPropertyMetadata(defaultValue, flags, (s, e) =>
            {
                if (propertyChangedCallback is null)
                {
                    return;
                }

                PropertyChangedEventArgs<TPropertyType> args = new(e.Property, e.NewValue, e.OldValue);
                TDependencyObject sender = s as TDependencyObject;
                propertyChangedCallback.Invoke(sender, args);

            }));
            return property;
        }


        #endregion

        #region

        public static DependencyProperty PropertyAttached<TPropertyType>(string propertyName, Type ownerType, Action<object, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null)
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (ownerType == null)
            {
                throw new ArgumentNullException(nameof(ownerType));
            }
            PropertyMetadata mata = new(default(TPropertyType), (s, e) =>
            {
                if (propertyChangedCallback is null)
                {
                    return;
                }

                PropertyChangedEventArgs<TPropertyType> args = new(e.Property, e.NewValue, e.OldValue);
                DependencyObject sender = s;
                propertyChangedCallback.Invoke(sender, args);

            });
            DependencyProperty property = DependencyProperty.RegisterAttached(propertyName, typeof(TPropertyType), ownerType, mata);
            return property;
        }


        public static DependencyProperty PropertyAttached<TPropertyType>(string propertyName, Type ownerType, TPropertyType defaultValue, Action<object, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null)
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (ownerType == null)
            {
                throw new ArgumentNullException(nameof(ownerType));
            }


            PropertyMetadata mata = new(defaultValue, (s, e) =>
            {
                if (propertyChangedCallback is null)
                {
                    return;
                }

                PropertyChangedEventArgs<TPropertyType> args = new(e.Property, e.NewValue, e.OldValue);
                DependencyObject sender = s;
                propertyChangedCallback.Invoke(sender, args);

            });


            DependencyProperty property = DependencyProperty.RegisterAttached(propertyName, typeof(TPropertyType), ownerType, mata);
            return property;
        }

        public static DependencyProperty PropertyAttached<TPropertyType>(string propertyName, Type ownerType, TPropertyType defaultValue, FrameworkPropertyMetadataOptions flags, Action<object, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null)
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (ownerType == null)
            {
                throw new ArgumentNullException(nameof(ownerType));
            }

            DependencyProperty property = DependencyProperty.RegisterAttached(propertyName, typeof(TPropertyType), ownerType, new FrameworkPropertyMetadata(defaultValue, flags, (s, e) =>
            {
                if (propertyChangedCallback is null)
                {
                    return;
                }

                PropertyChangedEventArgs<TPropertyType> args = new(e.Property, e.NewValue, e.OldValue);
                DependencyObject sender = s;
                propertyChangedCallback.Invoke(sender, args);

            }, null, false));
            return property;
        }


        public static DependencyProperty PropertyAttached<TPropertyType>(string propertyName, Type ownerType, TPropertyType defaultValue, FrameworkPropertyMetadataOptions flags, UpdateSourceTrigger updateSourceTrigger, Action<object, PropertyChangedEventArgs<TPropertyType>> propertyChangedCallback = null)
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (ownerType == null)
            {
                throw new ArgumentNullException(nameof(ownerType));
            }

            DependencyProperty property = DependencyProperty.RegisterAttached(propertyName, typeof(TPropertyType), ownerType, new FrameworkPropertyMetadata(defaultValue, flags, (s, e) =>
            {
                if (propertyChangedCallback is null)
                {
                    return;
                }

                PropertyChangedEventArgs<TPropertyType> args = new(e.Property, e.NewValue, e.OldValue);
                DependencyObject sender = s;
                propertyChangedCallback.Invoke(sender, args);

            }, null, false, updateSourceTrigger));
            return property;
        }


        #endregion

        /// <summary>
        /// RoutedEvent
        /// public static readonly RoutedEvent NameRoutedEvent = ElementBase.RoutedEvent<T,T>(string,T);
        /// public event EventHandler Name { add { AddHandler(EventHandler, value); } remove { RemoveHandler(EventHandler, value); } }
        /// </summary>
        /// <typeparam name="thisType"></typeparam>
        /// <typeparam name="propertyType"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RoutedEvent RoutedEvent<thisType, propertyType>(string name)
        {
            return EventManager.RegisterRoutedEvent(name.Replace("Event", ""), RoutingStrategy.Bubble, typeof(propertyType), typeof(thisType));
        }




        /// <summary>
        /// PropertyAssist.DefaultStyle<thisType>(DefaultStyleKeyProperty); 
        /// </summary>
        /// <typeparam name="thisType">this</typeparam>
        /// <param name="dp">DefaultStyleKeyProperty</param>
        public static void DefaultStyle<thisType>(DependencyProperty dp)
        {
            dp.OverrideMetadata(typeof(thisType), new FrameworkPropertyMetadata(typeof(thisType)));
        }







        [DebuggerDisplay("property:{Property.Name}  new value:{NewValue}  old value:{OldValue}")]
        public class PropertyChangedEventArgs<TargetType> : EventArgs
        {
            internal PropertyChangedEventArgs(DependencyProperty property, object newValue, object oldValue)
            {
                Property = property;
                OldValue = Invoker.CastTo<TargetType>(oldValue);
                NewValue = Invoker.CastTo<TargetType>(newValue);
            }


            public DependencyProperty Property { get; }
            public TargetType OldValue { get; }
            public TargetType NewValue { get; }
        }
    }
}
