namespace mm.flow
{
    public class RepeatTask : TaskNodeBase
    {
        private ITask task;

        public RepeatTask(ITask task)
        {
            this.task = task;
        }

        protected override void OnTaskEnterImpl(TaskRunner runner)
        {
            runner.Run(task);
        }

        protected override void OnTaskEndImpl(TaskRunner runner)
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

        protected override void TaskUpdateImpl(TaskRunner runner)
        {
            if (task.IsCompleted)
            {
                runner.End(task);
                runner.Run(task);
            }
        }
    }
}
