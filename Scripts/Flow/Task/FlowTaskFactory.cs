using System;
using System.Collections.Generic;

namespace mm.flow
{
    public static class FlowTaskFactory
    {
        public static ITask CreateAction(Action action) => new ActionTask(action);

        public static ITask CreateWait(float duration) => new WaitTask(duration);

        public static ParallelTask CreateParallel(this TaskRunner runner) => new ParallelTask();

        public static SequenceTask CreateSequence(this TaskRunner runner) => new SequenceTask();

        public static RepeatTask CreateRepeat(TaskRunner runner, ITask task) => new RepeatTask(task);

        public static SequenceTask Add(this SequenceTask sequence, ITask task)
        {
            sequence.List.Add(task);
            return sequence;
        }

        public static ParallelTask Add(this ParallelTask parallel, ITask task)
        {
            parallel.List.Add(task);
            return parallel;
        }
    }
}
