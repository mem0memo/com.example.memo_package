namespace mm.core
{
    public static class UseCaseSystem
    {
        public interface IFactory
        {
            public T Create<T>() where T : IUseCase, new();
        }

        public interface IUseCase
        {
            void Execute();
        }
    }
}