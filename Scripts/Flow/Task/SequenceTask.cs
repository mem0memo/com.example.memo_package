using System.Collections.Generic;
using System.Linq;

namespace mm.flow
{
    public class SequenceTask : TaskNodeBase
    {
        private List<ITask> taskList;
        private Queue<ITask> queue;
        private ITask current;

        public SequenceTask()
        {
            taskList = new List<ITask>();
            queue = new Queue<ITask>();
        }

        public SequenceTask(IEnumerable<ITask> tasks)
        {
            taskList = new List<ITask>(tasks);
            queue = new Queue<ITask>();
        }

        public IList<ITask> List => taskList;

        protected override void OnTaskEnterImpl(TaskRunner runner)
        {
            queue.Clear();
            foreach (var task in taskList)
            {
                queue.Enqueue(task);
            }

            if (queue.Count > 0)
            {
                current = queue.Dequeue();
                runner.Run(current);
            }
        }


        protected override void OnTaskEndImpl(TaskRunner runner)
        {
            if (taskList.All(task => task.IsCompleted))
            {
                Success();
                return;
            }
            else
            {
                foreach (var task in taskList)
                {
                    if (!task.IsCompleted)
                    {
                        runner.End(task);
                        Fail();
                    }
                }

                queue.Clear();
            }
        }

        protected override void TaskUpdateImpl(TaskRunner runner)
        {
            if (current.IsCompleted)
            {
                if (queue.TryDequeue(out var next))
                {
                    runner.Run(next);
                    current = next;
                    return;
                }
                else
                {
                    Success();
                }
            }
        }
    }
}
