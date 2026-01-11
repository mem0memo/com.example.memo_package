using System;
using System.Collections.Generic;

namespace mm
{
    public class FiniteStateTable
    {
        private Dictionary<Type, Func<Type>> transitionDict;

        public FiniteStateTable()
        {
            transitionDict = new Dictionary<Type, Func<Type>>();
        }

        public IDictionary<Type, Func<Type>> Transition => transitionDict;

        public Type Next<TCurrent>()
        {
            return GetNext(typeof(TCurrent));
        }

        private Type GetNext(Type type)
        {
            if (type == null)
            {
                return default;
            }

            transitionDict.TryGetValue(type, out var next);
            return next == null ? default : next?.Invoke();
        }
    }
}
