namespace mm
{
    public interface ITask
    {
        bool IsCompleted { get; }

        void OnTaskEnter();
        void OnTaskEnd();
        void TaskUpdate(double deltaTime);
    }
}
