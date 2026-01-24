namespace mm
{
    public interface ITaskService : IService
    {
        void Run(ITask task);
    }
}
