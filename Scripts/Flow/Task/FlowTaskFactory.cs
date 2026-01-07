using System;

namespace mm.flow
{
    public class FlowTaskFactory : ServiceComponentBase
    {
        public ITask CreateAction(Action action)
            => new ActionTask(action);

        public ITask CreateWait(float duration)
            => new WaitTask(duration);

        public ParallelTask CreateParallel()
            => new ParallelTask();

        public SequenceTask CreateSequence()
            => new SequenceTask();

        public RepeatTask CreateRepeat(ITask task)
            => new RepeatTask(task);
    }
}
