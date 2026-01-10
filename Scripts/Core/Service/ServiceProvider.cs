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

        public void Register<T>(T service) where T : IService
            => serviceDict[typeof(T)] = service;

        public T GetService<T>()
            where T : IService
        {
            if (TryGet<T>(out var result))
            {
                return result;
            }
            else if (TryGetWithType(out result))
            {
                serviceDict[typeof(T)] = result;
                return result;
            }
            else if (linkedParent != null)
            {
                return linkedParent.GetService<T>();
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
