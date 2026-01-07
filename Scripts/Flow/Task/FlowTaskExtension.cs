namespace mm.flow
{
    public static class FlowTaskExtension
    {
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
