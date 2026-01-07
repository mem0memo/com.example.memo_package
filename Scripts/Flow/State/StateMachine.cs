namespace mm.flow
{
    public class StateMachine
    {
        private IState current;

        public void Set(IState state)
        {
            var changed = current != state;
            if (changed)
            {
                current?.OnStateEnd();
                this.current = state;
                current?.OnStateEnter();
            }
        }

        public void Update(double deltaTime)
        {
            current?.StateUpdate(deltaTime);
        }
    }
}