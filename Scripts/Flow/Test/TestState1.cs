using System;

namespace mm.flow
{
    public class TestState1 : StateBase
    {
        public TestState1(Context args) : base(args)
        {
        }

        protected override void OnStateEnterImpl(Context context)
        {
            var logService = context.ServiceProvider.GetService<TestLogService>();
            logService.LogStart("sate1");

            var sequence = context.TaskRunner.CreateSequence();
            sequence
                .Add(FlowTaskFactory.CreateAction(() => logService.LogStart("TestState1")))
                .Add(FlowTaskFactory.CreateWait(1))
                .Add(FlowTaskFactory.CreateAction(logService.LogA))
                .Add(FlowTaskFactory.CreateWait(0.5f))
                .Add(FlowTaskFactory.CreateAction(logService.LogB))
                .Add(FlowTaskFactory.CreateWait(0.5f))
                .Add(FlowTaskFactory.CreateAction(logService.LogC))
                .Add(FlowTaskFactory.CreateWait(2))
                .Add(FlowTaskFactory.CreateAction(() => logService.LogEnd("TestState2")))
                .Add(FlowTaskFactory.CreateAction(Complete));

            context.TaskRunner.Run(sequence);
        }

        protected override void OnStateEndImpl(Context context)
        {
            var logService = context.ServiceProvider.GetService<TestLogService>();
            logService.LogEnd("state1");
        }
    }
}
