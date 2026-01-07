namespace mm.flow
{
    public class RepeatTask : RunnerTaskBase
    {
        private ITask task;

        public RepeatTask(ITask task)
        {
            this.task = task;
        }

        protected override void OnTaskEnterImpl(ITaskRunner runner)
        {
            runner.Run(task);
        }

        protected override void OnTaskEndImpl(ITaskRunner runner)
        {
            if (task.IsCompleted)
            {
                Success();
                return;
            }
            else
            {
                runner.End(task);
                Fail();
            }
        }

        protected override void TaskUpdateImpl(ITaskRunner runner, double deltaTime)
        {
            if (task.IsCompleted)
            {
                runner.End(task);
                runner.Run(task);
            }
        }
    }
}
