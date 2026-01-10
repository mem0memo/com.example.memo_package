namespace mm
{
    public class StateMachine
    {
        private IState current;

        public void Change(IState state)
        {
            var changed = current != state;
            if (changed)
            {
                current?.OnStateEnd();
                this.current = state;
                current?.OnStateEnter();
            }
        }

        public void End()
        {
            current?.OnStateEnd();
            current = default;
        }

        public void Update(double deltaTime)
        {
            if (current == null)
            {
                return;
            }
            else
            {
                current.StateUpdate(deltaTime);
                if (current.IsCompleted)
                {
                    current?.OnStateEnd();
                    current = default;
                }
            }
        }
    }
}