using UnityEngine;

namespace mm.flow
{
    public class TestLogService : ServiceComponentBase
    {
        public void LogStart(string message) => Debug.Log($"Start : {message}");

        public void LogEnd(string message) => Debug.Log($"End : {message}");

        public void LogA() => Debug.Log("A");

        public void LogB() => Debug.Log("B");

        public void LogC() => Debug.Log("C");

        public void Log(string message) => Debug.Log(message);
    }
}
