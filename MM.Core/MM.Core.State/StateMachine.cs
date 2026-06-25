namespace mm.core.state
{
    public class StateMachine
    {
        private IState current;

        public void Start(IState state)
        {
            current?.Stop();
            this.current = state;
            current?.Start();
        }

        public void Update(double deltaTime)
        {
            current?.Update(deltaTime);
        }
    }
}
