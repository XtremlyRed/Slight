using System.Diagnostics;
namespace Slight.Core
{
    [DebuggerDisplay("{Parameters}")]
    public class NavigationParameter
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<string, object> Parameters = new();

        public NavigationParameter SetValue<Target>(string targetKey, Target target)
        {
            if (string.IsNullOrWhiteSpace(targetKey))
            {
                throw new ArgumentNullException(nameof(targetKey));
            }
            Parameters[key: targetKey] = target;

            return this;
        }

        public Target GetValue<Target>(string targetKey)
        {
            if (Parameters.TryGetValue(targetKey, out object targetValue) == false)
            {
                throw new ArgumentException($"{targetKey} is not exist");
            }

            if (targetValue is Target target)
            {
                return target;
            }

            throw new Exception($"Unable to cast object of type '{targetValue.GetType()}' to type '{typeof(Target)}'.");
        }

        public NavigationParameter GetValue<Target>(string targetKey, out Target outTarget)
        {
            if (Parameters.TryGetValue(targetKey, out object targetValue))
            {
                if (targetValue is Target target)
                {
                    outTarget = target;
                    return this;
                }

                throw new Exception($"Unable to cast object of type '{targetValue.GetType()}' to type '{typeof(Target)}'.");

            }

            throw new ArgumentException($"{targetKey} is not exist");
        }
    }


    public interface INavigationParameterAware
    {
        public void Navigated(NavigationParameter parameter);
    }
}
