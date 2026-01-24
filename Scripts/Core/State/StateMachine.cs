namespace mm
{
    public class StateMachine
    {
        private IState current;

        public IState Current => current;

        public void Set(IState next)
        {
            var changed = current != next;
            if (changed)
            {
                var prev = current;
                current?.OnStateEnd();
                this.current = next;
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
            }
        }
    }
}