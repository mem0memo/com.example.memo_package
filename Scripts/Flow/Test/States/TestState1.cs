namespace mm.flow
{
    public class TestState1 : FlowStateBase
    {
        private FlowTaskFactory Flow => GetService<FlowTaskFactory>();

        private TestTaskFactory Test => GetService<TestTaskFactory>();

        protected override void OnStateEnterImpl()
        {
            RunTask(Test.Message("start : state1"));

            var sequence = Flow.CreateSequence();
            sequence
                .Add(Test.Message("TestState1"))
                .Add(Flow.CreateWait(1))
                .Add(Test.Message("AA"))
                .Add(Flow.CreateWait(0.5f))
                .Add(Test.Message("BB"))
                .Add(Flow.CreateWait(0.5f))
                .Add(Test.Message("CC"))
                .Add(Flow.CreateWait(2))
                .Add(Test.Message("TestState1"))
                .Add(Flow.CreateAction(Complete));

            RunTask(sequence);
        }

        protected override void OnStateEndImpl()
        {
            RunTask(Test.Message("end : state1"));
        }
    }
}
