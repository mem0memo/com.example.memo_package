using System;

namespace mm.flow
{
    public class ActionTask : TaskBase
    {
        private Action action;

        public ActionTask(Action action)
        {
            this.action = action;
        }

        protected override void TaskUpdateImpl(double deltaTime)
        {
            action?.Invoke();
            Complete();
        }
    }
}
