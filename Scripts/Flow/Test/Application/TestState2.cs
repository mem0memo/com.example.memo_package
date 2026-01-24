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

        public void OnStateEnter()
        {
            message.Send("state2", UnityEngine.Color.white);
        }

        public void OnStateEnd()
        {
            message.Send("end state2", UnityEngine.Color.gray);
        }

        public void StateUpdate(double deltaTime)
        {
        }
    }
}
