using System.Collections.Generic;

namespace mm.flow
{
    public class TaskRunner
    {
        private List<ITask> addListA;
        private List<ITask> addListB;

        private List<ITask> taskList;
        private List<ITask> removeList;
        private AddMode addMode;

        public TaskRunner()
        {
            addListA = new List<ITask>();
            addListB = new List<ITask>();
            taskList = new List<ITask>();
            removeList = new List<ITask>();
        }

        private enum AddMode
        {
            A,
            B,
        }

        public void Run(ITask task) => AddTask(task);

        public void End(ITask task)
        {
            if (taskList.Contains(task))
            {
                task.OnTaskEnd();
                taskList.Remove(task);
            }
        }

        public void Update(double deltaTime)
        {
            //addMode
            addMode = addMode switch
            {
                AddMode.B => AddMode.A,
                AddMode.A => AddMode.B,
                _ => AddMode.A,
            };

            //update
            foreach (var task in taskList)
            {
                task.TaskUpdate(deltaTime);
                if (task.IsCompleted)
                {
                    removeList.Add(task);
                    continue;
                }
            }

            //remove
            foreach (var remove in removeList)
            {
                if (taskList.Contains(remove))
                {
                    remove?.OnTaskEnd();
                    taskList.Remove(remove);
                }
            }

            removeList.Clear();

            //add
            var addList = addMode switch
            {
                AddMode.A => addListA,
                AddMode.B => addListB,
                _ => addListA,
            };

            foreach (var add in addList)
            {
                if (taskList.Contains(add))
                {
                    continue;
                }

                add.OnTaskEnter();
                taskList.Add(add);
            }

            addList.Clear();
        }

        private void AddTask(ITask task)
        {
            var addList = addMode switch
            {
                AddMode.A => addListB,
                AddMode.B => addListA,
                _ => default,
            };

            addList.Add(task);
        }
    }
}
