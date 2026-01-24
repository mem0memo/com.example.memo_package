using UnityEngine;

namespace mm.flow
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private FlowController flowController;
        [SerializeField]
        private TestLogger testLogger;

        private ServiceLocator serviceProvider;

        public enum TestStateType
        {
            State1,
            State2,
        }

        private void Awake()
        {
            serviceProvider = new ServiceLocator();
            serviceProvider.Register(flowController);
            serviceProvider.Register(testLogger);

            var state1 = new TestState1(serviceProvider);
            var state2 = new TestState2(serviceProvider) { NextIndex = (int)Test.TestStateType.State1 };
            flowController.Register((int)TestStateType.State1, state1);
            flowController.Register((int)TestStateType.State2, state2);
        }

        [ContextMenu("State1")]
        public void State1() => flowController.Set((int)TestStateType.State1);

        [ContextMenu("State2")]
        public void State2() => flowController.Set((int)TestStateType.State2);
    }
}
