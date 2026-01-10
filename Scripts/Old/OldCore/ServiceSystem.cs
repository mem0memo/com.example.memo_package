using System;

namespace mm.core
{
    public static class ServiceSystem
    {
        public interface IService
        {
        }

        public interface IResolver
        {
            T Resolve<T>() where T : IService;
            bool TryResolve<T>(out T result) where T : IService;
        }
    }
}