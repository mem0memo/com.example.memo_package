namespace mm.flow
{
    public class TestState1 : FlowStateBase
    {
        private FlowTaskFactory taskFactory;
        private TestLogService logService;
        private FlowManager flowManager;

        public TestState1(ServiceProvider serviceProvider)
        {
            serviceProvider.TryResolve(out taskFactory);
            serviceProvider.TryResolve(out logService);
            serviceProvider.TryResolve(out flowManager);
        }

        protected override void OnStateEnterImpl()
        {
            flowManager.Run(new TestMessageTask(logService) { Message = "start : state1" });

            var sequence = taskFactory.CreateSequence();
            sequence
                .Add(new TestMessageTask(logService) { Message = "TestState1" })
                .Add(taskFactory.CreateWait(1))
                .Add(new TestMessageTask(logService) { Message = "AA" })
                .Add(taskFactory.CreateWait(0.5f))
                .Add(new TestMessageTask(logService) { Message = "BB" })
                .Add(taskFactory.CreateWait(0.5f))
                .Add(new TestMessageTask(logService) { Message = "CC" })
                .Add(taskFactory.CreateWait(2))
                .Add(new TestMessageTask(logService) { Message = "TestState1" })
                .Add(taskFactory.CreateAction(Complete));

            flowManager.Run(sequence);
        }

        protected override void OnStateEndImpl()
        {
            flowManager.Run(new TestMessageTask(logService) { Message = "end : state1" });
        }
    }
}
