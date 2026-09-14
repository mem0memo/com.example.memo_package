using mm.core.state;

namespace mm.core.flow
{
    public class FlowStateBase<TController, TFlowType> : StateBase
    where TController : class
    {
        public FlowStateBase(IDIResolver dIResolver)
        {
            this.Resolver = dIResolver;
            this.Controller = dIResolver.Resolve<TController>();
            this.Flow = dIResolver.Resolve<IFlowController<TFlowType>>();
        }

        protected TController Controller { get; private set; }

        protected IDIResolver Resolver { get; private set; }

        protected IFlowController<TFlowType> Flow { get; private set; }
    }
}
