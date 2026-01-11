namespace mm.flow
{
    public class TestState2 : FlowStateBase
    {
        private FlowTaskFactory taskFactory;
        private TestLogService logService;
        private FlowManager flowManager;

        public TestState2(ServiceProvider serviceProvider)
        {
            serviceProvider.TryResolve(out taskFactory);
            serviceProvider.TryResolve(out logService);
            serviceProvider.TryResolve(out flowManager);
        }
        protected override void OnStateEnterImpl()
        {
            flowManager.Run(new TestMessageTask(logService) { Message = "start : state2" });

            var sequence = taskFactory.CreateSequence()
                .Add(new TestLogTask(logService))
                .Add(taskFactory.CreateAction(Complete));

            flowManager.Run(sequence);
        }

        protected override void OnStateEndImpl()
        {
            flowManager.Run(new TestMessageTask(logService) { Message = "end : state2" });
        }
    }
}
