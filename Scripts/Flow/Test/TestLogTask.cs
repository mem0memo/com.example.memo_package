namespace mm.flow
{
    public class TestLogTask : TaskBase
    {
        private TestLogService logService;

        public TestLogTask(TestLogService logService)
        {
            this.logService = logService;
        }

        protected override void OnTaskEnterImpl()
        {
            logService.LogStart("TestLogTask");
            logService.LogA();
            logService.LogB();
            logService.LogC();
            logService.LogEnd("TestLogTask");
        }
    }
}
