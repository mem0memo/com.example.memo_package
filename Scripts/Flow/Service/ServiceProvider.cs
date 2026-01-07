using System.Collections.Generic;
using UnityEngine;

namespace mm.flow
{
    public class ServiceProvider : MonoBehaviour
    {
        [SerializeField]
        private List<ServiceBehaviourBase> serviceList;

        public T GetService<T>()
            where T : ServiceBehaviourBase
        {
            if (TryGetService<T>(out var service))
            {
                return service;
            }

            throw new KeyNotFoundException($"Service not found: {typeof(T)}");
        }

        private bool TryGetService<T>(out T service)
            where T : ServiceBehaviourBase
        {
            foreach (var svc in serviceList)
            {
                if (svc is T typedSvc)
                {
                    service = typedSvc;
                    return true;
                }
            }

            service = null;
            return false;
        }
    }
}
