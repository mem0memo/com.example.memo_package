namespace mm.core.flow
{
    public interface IFlowController<TType> : IInfrastructure
    {
        TType CurrentFlow { get; }

        void StartFlow(TType type);
    }
}
