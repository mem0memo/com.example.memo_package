namespace mm.core.state
{
    public class StateBase : IState
    {
        public void Start()
        {
            OnStart();
        }

        public void Stop()
        {
            OnStop();
        }

        public void Update(double deltaTime)
        {
            OnUpdate(deltaTime);
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop()
        {
        }

        protected virtual void OnUpdate(double deltaTime)
        {
        }
    }
}