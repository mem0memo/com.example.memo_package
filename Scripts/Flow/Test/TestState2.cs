namespace mm.flow
{
    public class TestState2 : StateBase
    {
        public TestState2(Context args) : base(args)
        {
        }

        protected override void OnStateEnterImpl(Context context)
        {
            var logService = context.ServiceProvider.GetService<TestLogService>();
            logService.LogStart("state2");

            var sequence = context.TaskRunner.CreateSequence()
                .Add(new TestLogTask(logService))
                .Add(FlowTaskFactory.CreateAction(Complete));

            context.TaskRunner.Run(sequence);
        }

        protected override void OnStateEndImpl(Context context)
        {
            var logService = context.ServiceProvider.GetService<TestLogService>();
            logService.LogEnd("state2");
        }
    }
}
