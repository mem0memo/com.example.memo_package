namespace mm.core.flow
{
    public interface IDIResolver
    {
        T Resolve<T>() where T : class;

        bool TryResolve<T>(out T result) where T : class
        {
            result = Resolve<T>();
            return result != null;
        }
    }
}
