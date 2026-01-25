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
                current?.OnQuit();
                this.current = next;
                current?.OnEnter();
            }
        }

        public void End()
        {
            current?.OnQuit();
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
                current.Tick(deltaTime);
            }
        }
    }
}