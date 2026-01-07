using System;

namespace mm.flow
{
    public abstract class TaskNodeBase : ITask
    {
        private StateType state;
        private TaskRunner runner;

        public TaskNodeBase(TaskRunner runner)
        {
            this.runner = runner;
        }

        public enum StateType
        {
            Success,
            Running,
            Failed,
        }

        public bool IsCompleted { get; private set; }

        public void OnTaskEnd()
        {
            OnTaskEndImpl(runner);
            IsCompleted = true;
            runner.Update(0);
        }

        public void OnTaskEnter()
        {
            IsCompleted = false;
            state = StateType.Running;
            OnTaskEnterImpl(runner);
            runner.Update(0);
        }

        public void TaskUpdate(double deltaTime)
        {
            if (!IsCompleted)
            {
                TaskUpdateImpl(runner);
            }

            runner.Update(deltaTime);
            IsCompleted = state != StateType.Running;
        }

        protected virtual void OnTaskEnterImpl(TaskRunner runner)
        {
        }

        protected virtual void TaskUpdateImpl(TaskRunner runner)
        {
        }

        protected virtual void OnTaskEndImpl(TaskRunner runner)
        {
        }

        protected void Running() => state = StateType.Running;

        protected void Success() => state = StateType.Success;

        protected void Fail() => state = StateType.Failed;
    }
}
