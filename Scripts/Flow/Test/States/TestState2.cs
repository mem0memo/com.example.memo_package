namespace mm.flow
{
    public class TestState2 : FlowStateBase
    {
        private FlowTaskFactory Flow => GetService<FlowTaskFactory>();

        private TestTaskFactory Test => GetService<TestTaskFactory>();

        protected override void OnStateEnterImpl()
        {
            RunTask(Test.Message("start : state2"));

            var sequence = Flow.CreateSequence()
                .Add(Test.Log())
                .Add(Flow.CreateAction(Complete));

            RunTask(sequence);
        }

        protected override void OnStateEndImpl()
        {
            RunTask(Test.Message("end : state2"));
        }
    }
}
