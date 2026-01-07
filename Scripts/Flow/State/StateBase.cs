using System;

namespace mm.flow
{
    public class StateBase : IState
    {
        private Context context;

        public StateBase(Context args)
        {
            this.context = args;
        }

        public bool IsCompleted { get; private set; }

        public void OnStateEnter()
        {
            IsCompleted = false;
            OnStateEnterImpl(context);
        }

        public void OnStateEnd()
        {
            OnStateEndImpl(context);
            IsCompleted = true;
        }

        public void StateUpdate(double deltaTime)
        {
            StateUpdateImpl(context, deltaTime);
        }

        protected void Complete() => IsCompleted = true;

        protected virtual void OnStateEnterImpl(Context context)
        {
        }

        protected virtual void OnStateEndImpl(Context context)
        {
        }

        protected virtual void StateUpdateImpl(Context context, double deltaTime)
        {
            IsCompleted = true;
        }

        public struct Context
        {
            public ServiceProvider ServiceProvider;
            public TaskRunner TaskRunner;
        }
    }
}
