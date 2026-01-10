namespace mm.core
{
    public class StateMachine
    {
        private StateSystem.IState current;

        public void Set(StateSystem.IState state)
        {
            var changed = current != state;
            if (changed)
            {
                current?.OnExit();
                this.current = state;
                current?.OnEntry();
            }
        }

        public void Update(TimeData timeData)
        {
            current?.Update(timeData);
        }
    }
}