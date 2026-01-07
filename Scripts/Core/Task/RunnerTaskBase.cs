namespace mm
{
    public abstract class RunnerTaskBase : ITask
    {
        private StateType state;
        private TaskRunner runner;

        public RunnerTaskBase()
        {
            this.runner = new TaskRunner();
        }

        public enum StateType
        {
            Success,
            Running,
            Failed,
        }

        public bool IsCompleted => state != StateType.Running;

        public void OnTaskEnter()
        {
            state = StateType.Running;
            OnTaskEnterImpl(runner);
            runner.Update(0);
        }

        public void TaskUpdate(double deltaTime)
        {
            if (!IsCompleted)
            {
                TaskUpdateImpl(runner, deltaTime);
            }

            runner.Update(deltaTime);
        }

        public void OnTaskEnd()
        {
            OnTaskEndImpl(runner);
            runner.Clear();
        }

        protected virtual void OnTaskEnterImpl(ITaskRunner runner)
        {
        }

        protected virtual void TaskUpdateImpl(ITaskRunner runner, double deltaTime)
        {
        }

        protected virtual void OnTaskEndImpl(ITaskRunner runner)
        {
        }

        protected void Running() => state = StateType.Running;

        protected void Success() => state = StateType.Success;

        protected void Fail() => state = StateType.Failed;
    }
}
