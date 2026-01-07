using System;

namespace mm.flow
{
    public class ConditionalTask : RunnerTaskBase
    {
        private Func<bool> judge;
        private ITask successTask;
        private ITask failedTask;

        private ITask current;

        public ConditionalTask(
            Func<bool> judge,
            ITask successTask,
            ITask failedTask)
        {
            this.judge = judge;
            this.successTask = successTask;
            this.failedTask = failedTask;
        }

        protected override void OnTaskEnterImpl(ITaskRunner runner)
        {
            this.current = judge.Invoke() ? successTask : failedTask;
            runner.Run(current);
        }

        protected override void OnTaskEndImpl(ITaskRunner runner)
        {
            if (current.IsCompleted)
            {
                return;
            }
            else
            {
                runner.End(current);
            }

            Fail();
        }

        protected override void TaskUpdateImpl(ITaskRunner runner, double deltaTime)
        {
            if (current.IsCompleted)
            {
                Success();
                return;
            }
        }
    }
}
