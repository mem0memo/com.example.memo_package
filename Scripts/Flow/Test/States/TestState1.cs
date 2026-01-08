namespace mm.flow
{
    public class TestState1 : FlowStateBase
    {
        private FlowTaskFactory Flow => GetService<FlowTaskFactory>();

        private TestLogService Log => GetService<TestLogService>();

        protected override void OnStateEnterImpl()
        {
            RunTask(new TestMessageTask(Log) { Message = "start : state1" });

            var sequence = Flow.CreateSequence();
            sequence
                .Add(new TestMessageTask(Log) { Message = "TestState1" })
                .Add(Flow.CreateWait(1))
                .Add(new TestMessageTask(Log) { Message = "AA" })
                .Add(Flow.CreateWait(0.5f))
                .Add(new TestMessageTask(Log) { Message = "BB" })
                .Add(Flow.CreateWait(0.5f))
                .Add(new TestMessageTask(Log) { Message = "CC" })
                .Add(Flow.CreateWait(2))
                .Add(new TestMessageTask(Log) { Message = "TestState1" })
                .Add(Flow.CreateAction(Complete));

            RunTask(sequence);
        }

        protected override void OnStateEndImpl()
        {
            RunTask(new TestMessageTask(Log) { Message = "end : state1" });
        }
    }
}
