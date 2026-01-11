using System;
using System.Collections.Generic;
using UnityEngine;

namespace mm
{
    public class ServiceProvider : MonoBehaviour
    {
        [SerializeField]
        private ServiceProvider[] links;

        private Dictionary<Type, IService> serviceDict = new Dictionary<Type, IService>();

        public void Register<T>(T service) where T : IService
            => serviceDict[typeof(T)] = service;

        public T Resolve<T>()
            where T : IService
        {
            if (TryResolve<T>(out var result))
            {
                return result;
            }

            throw new KeyNotFoundException($"Service not found: {typeof(T)}");
        }

        public bool TryResolve<T>(out T result)
            where T : IService
        {
            if (TryGet(out result))
            {
                return true;
            }
            else if (TryGetWithType(out result))
            {
                serviceDict[typeof(T)] = result;
                return true;
            }
            else
            {
                foreach (var link in links)
                {
                    if (link.TryResolve(out result))
                    {
                        return true;
                    }
                }
            }

            return false;
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

        private bool TryGetWithType<TService>(out TService result)
        {
            foreach (var value in serviceDict.Values)
            {
                if (value is TService service)
                {
                    result = (TService)value;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
