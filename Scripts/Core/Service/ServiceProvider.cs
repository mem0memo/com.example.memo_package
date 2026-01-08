using System;
using System.Collections.Generic;
using UnityEngine;

namespace mm
{
    public class ServiceProvider : MonoBehaviour
    {
        [SerializeField]
        private ServiceProvider linkedParent;

        private Dictionary<Type, IService> serviceDict;
        private ServiceProvider Parent => linkedParent;

        public void Register<T>(T service) where T : IService
            => serviceDict[typeof(T)] = service;

        public T GetService<T>()
            where T : IService
        {
            if (TryGet<T>(out var result))
            {
                return (T)result;
            }
            else
            {
                foreach (var value in serviceDict.Values)
                {
                    if (value is T service)
                    {
                        serviceDict[typeof(T)] = value;
                        return service;
                    }
                }

                if (Parent != null && Parent.TryGet<T>(out result))
                {
                    return result;
                }
            }

            throw new KeyNotFoundException($"Service not found: {typeof(T)}");
        }

        private void Awake()
        {
            serviceDict = new Dictionary<Type, IService>();
            var components = GetComponents<IService>();

            foreach (var component in components)
            {
                if (component is IService service)
                {
                    serviceDict[service.GetType()] = service;
                }
            }
        }

        private bool TryGet<TService>(out TService result)
        {
            if (serviceDict.TryGetValue(typeof(TService), out var service))
            {
                result = (TService)service;
                return true;
            }

            result = default;
            return false;
        }
    }
}
