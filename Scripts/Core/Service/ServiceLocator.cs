using System;
using System.Collections.Generic;

namespace mm
{
    public class ServiceLocator
    {
        private Dictionary<Type, IService> serviceDict;

        public ServiceLocator()
        {
            serviceDict = new Dictionary<Type, IService>();
        }

        public ServiceLocator(IEnumerable<IService> services)
        {
            serviceDict = new Dictionary<Type, IService>();
            foreach (var service in services)
            {
                serviceDict[service.GetType()] = service;
            }
        }

        public IEnumerable<IService> Gets() => serviceDict.Values;

        public void Register(IService service) => serviceDict[service.GetType()] = service;

        public void Register(IEnumerable<IService> services)
        {
            foreach (var service in services)
            {
                serviceDict[service.GetType()] = service;
            }
        }

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

            return false;
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
