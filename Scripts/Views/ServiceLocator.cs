
using mm.core;

namespace mm.view
{
    public class ServiceLocator :
        RootComponentBase<ServiceSystem.IService>,
        ServiceSystem.IResolver
    {
        T ServiceSystem.IResolver.Resolve<T>()
        {
            TryFind<T>(out var result);
            return result;
        }

        bool ServiceSystem.IResolver.TryResolve<T>(out T result) => TryFind(out result);

        private void Awake()
        {
            Initialize();
        }
    }
}