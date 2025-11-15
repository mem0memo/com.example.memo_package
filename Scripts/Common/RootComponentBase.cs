using System.Collections.Generic;
using UnityEngine;

namespace mm.common
{
    public abstract class RootComponentBase<TComponent> : MonoBehaviour
    {
        protected List<TComponent> list = new List<TComponent>();
        private bool initialized = false;

        public bool TryFind<T>(out T result)
        where T : TComponent
        {
            Initialize();
            foreach (var c in list)
            {
                if (c is T t)
                {
                    result = t;
                    return true;
                }
            }

            result = default;
            return false;
        }

        protected void Initialize()
        {
            if (!initialized)
            {
                GetComponents(list);
                initialized = true;
            }
        }
    }
}