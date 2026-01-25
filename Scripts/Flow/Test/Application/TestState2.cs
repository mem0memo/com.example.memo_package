namespace mm.flow
{
    public class TestState2 : IState
    {
        private IMessageService message;

        public TestState2(ServiceLocator serviceProvider)
        {
            serviceProvider.TryResolve(out message);
        }

        public int NextIndex { get; set; }

        public void OnEnter()
        {
            message.Send("state2", UnityEngine.Color.white);
        }

        public void OnQuit()
        {
            message.Send("end state2", UnityEngine.Color.gray);
        }

        public void Tick(double deltaTime)
        {
        }
    }
}
