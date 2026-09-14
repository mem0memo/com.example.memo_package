namespace mm.core.state
{
    public class StateMachine
    {
        private IState current;

        public void Start(IState state)
        {
            current?.Exit();
            this.current = state;
            current?.Enter();
        }

        public void Update(double deltaTime)
        {
            current?.ProcessFrame(deltaTime);
        }
    }
}
