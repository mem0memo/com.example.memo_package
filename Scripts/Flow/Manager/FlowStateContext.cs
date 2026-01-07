namespace mm.flow
{
    public struct FlowStateContext
    {
        public ServiceProvider ServiceProvider;
        public ITaskRunner TaskRunner;

        public bool IsValid()
            => ServiceProvider != null
                && TaskRunner != null;
    }
}
