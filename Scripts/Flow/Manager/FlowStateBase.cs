namespace mm.flow
{
    public abstract class FlowStateBase : IState
    {
        private FlowStateContext context;

        public bool IsCompleted { get; private set; }

        public void SetContext(FlowStateContext context)
        {
            this.context = context;
        }

        public void OnStateEnter()
        {
            IsCompleted = false;
            OnStateEnterImpl();
        }

        public void OnStateEnd()
        {
            OnStateEndImpl();
            IsCompleted = true;
        }

        public void StateUpdate(double deltaTime)
        {
            StateUpdateImpl(deltaTime);
        }

        protected void Complete() => IsCompleted = true;

        protected TService GetService<TService>()
            where TService : IService
            => context.IsValid() ? context.ServiceProvider.Resolve<TService>() : default;

        protected void RunTask(ITask task)
        {
            if (context.IsValid())
            {
                context.TaskRunner.Run(task);
            }
        }

        protected virtual void OnStateEnterImpl()
        {
        }

        protected virtual void OnStateEndImpl()
        {
        }

        protected virtual void StateUpdateImpl(double deltaTime)
        {
        }
    }
}
