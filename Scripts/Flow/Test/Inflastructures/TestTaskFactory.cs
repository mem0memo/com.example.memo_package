using UnityEngine;

namespace mm.flow
{
    public class TestTaskFactory : ServiceComponentBase
    {
        [SerializeField]
        private ServiceProvider serviceProvider;

        public TestLogTask Log()
        {
            var logService = serviceProvider.GetService<TestLogService>();
            return new TestLogTask(logService);
        }

        public TestMessageTask Message(string message)
        {
            var logService = serviceProvider.GetService<TestLogService>();
            return new TestMessageTask(logService) { Message = message };
        }
    }
}
