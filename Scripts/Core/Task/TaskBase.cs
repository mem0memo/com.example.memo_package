namespace mm
{
    public class TaskBase : ITask
    {
        public bool IsCompleted { get; private set; }

        public void OnTaskEnd()
        {
            OnTaskEndImpl();
            IsCompleted = true;
        }

        public void OnTaskEnter()
        {
            IsCompleted = false;
            OnTaskEnterImpl();
        }

        public void TaskUpdate(double deltaTime)
        {
            TaskUpdateImpl(deltaTime);
        }

        public void Complete() => IsCompleted = true;

        protected virtual void OnTaskEnterImpl()
        {
        }

        protected virtual void TaskUpdateImpl(double deltaTime)
        {
            IsCompleted = true;
        }

        protected virtual void OnTaskEndImpl()
        {
        }
    }
}
