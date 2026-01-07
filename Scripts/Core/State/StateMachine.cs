using System;

namespace mm
{
    public class StateMachine
    {
        private IState current;

        public Action<IState> Completed { get; set; }

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
                    var prev = current;
                    current?.OnStateEnd();
                    current = null;
                    Completed?.Invoke(prev);
                }
            }
        }
    }
}