namespace mm.core.state
{
    public class StateBase : IState
    {
        public void Enter()
        {
            OnEntry();
        }

        public void Exit()
        {
            OnExit();
        }

        public void ProcessFrame(double deltaTime)
        {
            ProcessFrame(deltaTime);
        }

        protected virtual void OnEntry()
        {
        }

        protected virtual void OnExit()
        {
        }

        protected virtual void OnProcessFrame(double deltaTime)
        {
        }
    }
}