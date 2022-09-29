using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Slight.Core
{
    internal static class ObjectBuilder
    {
        #region Container items
        // Compiles a lambda that calls the given type's first constructor resolving arguments
        internal static Func<ILifetime, object> FactoryFromType(Type itemType)
        {
            // Get first constructor for the type
            ConstructorInfo[] constructors = itemType.GetConstructors();
            if (constructors.Length == 0)
            {
                // If no public constructor found, search for an internal constructor
                constructors = itemType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            }
            ConstructorInfo constructor = constructors.First();

            // Compile constructor call as a lambda expression
            ParameterExpression arg = Expression.Parameter(typeof(ILifetime));
            return (Func<ILifetime, object>)Expression.Lambda(
                Expression.New(constructor, constructor.GetParameters().Select(
                    param =>
                    {
                        Func<ILifetime, object> resolve = new(
                            lifetime => lifetime.Resolve(param.ParameterType));
                        return Expression.Convert(
                            Expression.Call(Expression.Constant(resolve.Target), resolve.Method, arg),
                            param.ParameterType);
                    })),
                arg).Compile();
        }
        #endregion
    }
}
