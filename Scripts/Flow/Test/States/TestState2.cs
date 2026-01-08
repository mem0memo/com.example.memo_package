namespace mm.flow
{
    public class TestState2 : FlowStateBase
    {
        private FlowTaskFactory Flow => GetService<FlowTaskFactory>();

        private TestLogService Log => GetService<TestLogService>();

        protected override void OnStateEnterImpl()
        {
            RunTask(new TestMessageTask(Log) { Message = "start : state2" });

            var sequence = Flow.CreateSequence()
                .Add(new TestLogTask(Log))
                .Add(Flow.CreateAction(Complete));

            RunTask(sequence);
        }

        protected override void OnStateEndImpl()
        {
            RunTask(new TestMessageTask(Log) { Message = "end : state2" });
        }
    }
}
