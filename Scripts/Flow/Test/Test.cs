using UnityEngine;

namespace mm.flow
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private FlowManager flowManager;

        private void Start()
        {
            flowManager.RegisterState<TestState1>();
            flowManager.RegisterState<TestState2>();
            flowManager.RegisterTransition<TestState2>(() => typeof(TestState1));
        }

        [ContextMenu("State1")]
        public void State1() => flowManager.Change<TestState1>();

        [ContextMenu("State2")]
        public void State2() => flowManager.Change<TestState2>();
    }
}
