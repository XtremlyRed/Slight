using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slight.Core
{
    [DebuggerDisplay("{Parameters}")]
    public class NavigateParameter
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<string, object> Parameters = new();

        public NavigateParameter SetValue<Target>(string targetKey, Target target)
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
            return Parameters.TryGetValue(targetKey, out object target1) ? (Target)target1 : throw new ArgumentException($"{targetKey} is not exist");
        }

        public NavigateParameter GetValue<Target>(string targetKey, out Target target)
        {
            if (Parameters.TryGetValue(targetKey, out object target1))
            {
                target = (Target)target1;

                return this;
            }

            throw new ArgumentException($"{targetKey} is not exist");
        }
    }
}
