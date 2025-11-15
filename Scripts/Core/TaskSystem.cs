using System;
using System.Collections.Generic;

namespace mm.core
{
    public static class TaskSystem
    {
        public interface ITask
        {
            bool IsFinished { get; }

            void OnTaskStart();

            void OnTaskEnd();

            void TaskUpdate(TimeData timeData);
        }

        public interface IProvider : ServiceSystem.IService
        {
            void Add(ITask task);

            TTask Add<TTask>()
            where TTask : ITask, new();
        }

        public class Updater
        {
            private List<ITask> addListA;
            private List<ITask> addListB;

            private List<ITask> taskList;
            private List<ITask> removeList;
            private AddMode addMode;

            public Updater()
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

            public void Add(ITask task) => AddTask(task);

            public void Destroy()
            {
                foreach (var task in taskList)
                {
                    task.OnTaskEnd();
                }
            }

            public void Update(TimeData timeData)
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
                    if (task.IsFinished)
                    {
                        removeList.Add(task);
                        continue;
                    }

                    task.TaskUpdate(timeData);
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

                    add.OnTaskStart();
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

        public class ActionTask : ITask
        {
            public Action Completed { get; set; }

            public bool IsFinished { get; private set; }

            public void OnTaskEnd() => Completed?.Invoke();

            public void OnTaskStart() => IsFinished = true;

            public void TaskUpdate(TimeData timeData)
            {
            }
        }

        public class QueueTask : ITask
        {
            private List<ITask> list;
            private Queue<ITask> queue;
            private ITask current;

            public bool IsFinished { get; private set; }

            public QueueTask()
            {
                list = new List<ITask>();
                queue = new Queue<ITask>();
            }

            public TTask Enqueue<TTask>()
            where TTask : ITask, new()
            {
                var task = new TTask();
                list.Add(task);
                return task;
            }

            public void OnTaskStart()
            {
                queue.Clear();
                foreach (var task in list)
                {
                    queue.Enqueue(task);
                }

                if (queue.TryDequeue(out current))
                {
                    current?.OnTaskStart();
                }

                IsFinished = false;
            }

            public void TaskUpdate(TimeData timeData)
            {
                if (current == null || IsFinished)
                {
                    return;
                }

                if (current.IsFinished)
                {
                    current?.OnTaskEnd();
                    if (queue.TryDequeue(out current))
                    {
                        current?.OnTaskStart();
                        return;
                    }
                    else
                    {
                        current = default;
                        IsFinished = true;
                        return;
                    }
                }
                else if (current != null)
                {
                    current.TaskUpdate(timeData);
                }
            }

            public void OnTaskEnd()
            {
                if (current != null)
                {
                    current.OnTaskEnd();
                }

                IsFinished = true;
            }
        }

        public class WaitTask : ITask
        {
            private double time;
            public double Timer { get; set; }

            public bool IsFinished { get; private set; }

            public void OnTaskStart()
            {
                time = 0;
                IsFinished = false;
            }

            public void TaskUpdate(TimeData timeData)
            {
                if (!IsFinished)
                {
                    if (Timer <= time)
                    {
                        IsFinished = true;
                        return;
                    }

                    time += timeData.DeltaTime;
                }
            }

            public void OnTaskEnd()
            {
                IsFinished = true;
            }
        }

        public class PauseTask : ITask
        {
            public bool IsFinished { get; private set; }

            public void Resume()
            {
                IsFinished = true;
            }

            public void OnTaskStart()
            {
                IsFinished = false;
            }

            public void OnTaskEnd()
            {
                IsFinished = true;
            }

            public void TaskUpdate(TimeData timeData)
            {
            }
        }
    }
}