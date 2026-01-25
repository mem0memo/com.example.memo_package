namespace mm.flow
{
    public class TestState1 : IState
    {
        private IMessageService message;

        public TestState1(ServiceLocator serviceProvider)
        {
            serviceProvider.TryResolve(out message);
        }

        public void OnEnter()
        {
            message.Send("state1", UnityEngine.Color.white);
        }

        public void OnQuit()
        {
            message.Send("end state1", UnityEngine.Color.gray);
        }

        public void Tick(double deltaTime)
        {
        }
    }
}
