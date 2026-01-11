using System;
using System.Collections.Generic;
namespace mm
{
    public class StackStateTable
    {
        private Stack<IState> stack;

        public StackStateTable()
        {
            this.stack = new Stack<IState>();
        }

        public void Push(IState state)
        {
            stack.Push(state);
        }

        public IState Pop()
        {
            return stack.Pop();
        }
    }
}
