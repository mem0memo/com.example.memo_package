namespace mm.flow
{
    public class TestMessageTask : TaskBase
    {
        private TestLogService logService;

        public TestMessageTask(TestLogService service)
        {
            this.logService = service;
        }

        public string Message { get; set; }

        protected override void OnTaskEnterImpl()
        {
            logService.Log(Message);
        }
    }
}
