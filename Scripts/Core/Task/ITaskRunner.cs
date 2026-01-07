namespace mm
{
    public interface ITaskRunner
    {
        void Run(ITask task);

        void End(ITask task);
    }
}
