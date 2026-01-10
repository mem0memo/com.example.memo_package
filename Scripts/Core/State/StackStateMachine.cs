using System.Collections.Generic;
namespace mm
{
    public class StackStateMachine
    {
        private Stack<IState> pauseStack;
        private IState current;

        public StackStateMachine()
        {
            this.pauseStack = new Stack<IState>();
        }

        public void Call(IState state)
        {
            if (current == state)
            {
                return;
            }

            if (current != null && !current.IsCompleted)
            {
                pauseStack.Push(current);
            }

            current = state;
            current?.OnStateEnter();
        }

        public void Return()
        {
            current?.OnStateEnd();
            pauseStack.TryPop(out current);
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
                if (current.IsCompleted)
                {
                    current?.OnStateEnd();
                    pauseStack.TryPop(out current);
                    current?.OnStateEnter();
                }
            }
        }
    }
}
