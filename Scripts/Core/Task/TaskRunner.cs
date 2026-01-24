using System.Collections.Generic;

namespace mm
{
    public class TaskRunner
    {
        private Queue<ITask> addQueue;
        private List<ITask> updateList;
        private Queue<ITask> removeQueue;

        public TaskRunner()
        {
            addQueue = new Queue<ITask>();
            updateList = new List<ITask>();
            removeQueue = new Queue<ITask>();
        }

        public void Run(ITask task) => addQueue.Enqueue(task);

        public void End(ITask task) => removeQueue.Enqueue(task);

        public void Clear()
        {
            addQueue.Clear();
            foreach (var update in updateList)
            {
                removeQueue.Enqueue(update);
            }
        }

        public void Update(double deltaTime)
        {
            //add
            while (addQueue.TryDequeue(out var add))
            {
                add.OnTaskEnter();
                updateList.Add(add);
            }

            //update
            foreach (var task in updateList)
            {
                task.TaskUpdate(deltaTime);
                if (task.IsCompleted)
                {
                    removeQueue.Enqueue(task);
                    continue;
                }
            }

            //remove
            while (removeQueue.TryDequeue(out var remove))
            {
                if (updateList.Contains(remove))
                {
                    remove?.OnTaskEnd();
                    updateList.Remove(remove);
                }
            }

            removeQueue.Clear();
        }
    }
}
