using System;
using System.Collections.Generic;
using UnityEngine;

namespace mm
{
    public class ServiceProvider : ServiceComponentBase
    {
        [SerializeField]
        private List<ServiceComponentBase> components = new List<ServiceComponentBase>();

        private Dictionary<Type, IService> serviceDict;

        public void Register<T>(T service) where T : IService
            => serviceDict[typeof(T)] = service;

        public T GetService<T>()
            where T : IService
        {
            if (serviceDict.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            foreach (var value in serviceDict.Values)
            {
                if (value is T result)
                {
                    serviceDict[typeof(T)] = value;
                    return result;
                }
            }

            throw new KeyNotFoundException($"Service not found: {typeof(T)}");
        }

        private void Awake()
        {
            serviceDict = new Dictionary<Type, IService>();
            var services = GetServicesRecursive();

            foreach (var service in services)
            {
                serviceDict[service.GetType()] = service;
            }
        }

        private IEnumerable<IService> GetServicesRecursive()
        {
            foreach (var component in components)
            {
                if (component is ServiceProvider provider)
                {
                    var services = provider.GetServicesRecursive();
                    foreach (var service in services)
                    {
                        yield return service;
                    }
                }
                else
                {
                    if (component is IService)
                    {
                        yield return component;
                    }
                }
            }
        }
    }
}
