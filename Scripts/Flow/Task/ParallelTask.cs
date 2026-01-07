using System.Collections.Generic;

namespace mm.flow
{
    public class ParallelTask : TaskNodeBase
    {
        private List<ITask> taskList;

        public ParallelTask()
        {
            taskList = new List<ITask>();
        }

        public ParallelTask(IEnumerable<ITask> tasks)
        {
            taskList = new List<ITask>(tasks);
        }

        public IList<ITask> List => taskList;

        protected override void OnTaskEnterImpl(TaskRunner runner)
        {
            foreach (var task in taskList)
            {
                runner.Run(task);
            }

            Running();
        }

        protected override void OnTaskEndImpl(TaskRunner runner)
        {
            bool allComplete = false;
            foreach (var task in taskList)
            {
                if (!task.IsCompleted)
                {
                    runner.End(task);
                    allComplete = false;
                }
            }

            if (allComplete)
            {
                Success();
            }
            else
            {
                Fail();
            }
        }

        protected override void TaskUpdateImpl(TaskRunner runner)
        {
            var allComplete = true;
            foreach (var task in taskList)
            {
                if (!task.IsCompleted)
                {
                    allComplete = false;
                }
            }

            if (allComplete)
            {
                Success();
            }
        }
    }
}
