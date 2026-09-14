namespace mm.core.flow
{
    public interface IFlowController<TType>
    {
        TType CurrentFlow { get; }

        void StartFlow(TType type);
    }
}
