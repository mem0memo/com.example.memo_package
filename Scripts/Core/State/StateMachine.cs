namespace mm
{
    public class StateMachine
    {
        private IState current;
        private IStateTable stateTable;

        public StateMachine(IStateTable table)
        {
            stateTable = table;
        }

        public StateMachine()
        {
            stateTable = new IStateTable.Empty();
        }

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
                stateTable.Change(prev, next);
            }
        }

        public void End()
        {
            current?.OnStateEnd();
            current = stateTable.Next(current);
            current?.OnStateEnter();
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