namespace mm.flow
{
    public class WaitTask : TaskBase
    {
        private double duration;
        private double timer;

        public WaitTask(double duration)
        {
            this.duration = duration;
        }

        protected override void OnTaskEnterImpl()
        {
            timer = 0;
        }

        protected override void TaskUpdateImpl(double deltaTime)
        {
            timer += deltaTime;
            if (duration <= timer)
            {
                Complete();
                return;
            }
        }
    }
}
