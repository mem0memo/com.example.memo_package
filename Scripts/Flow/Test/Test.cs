using UnityEngine;

namespace mm.flow
{
    public class Test : MonoBehaviour
    {
        private IState state1;
        private IState state2;
        private IState state3;

        [SerializeField]
        private FlowManager flowManager;
        [SerializeField]
        private bool test3;

        private void Start()
        {
            state1 = flowManager.CreateState<TestState1>();
            state2 = flowManager.CreateState<TestState2>();
            state3 = flowManager.CreateState<TestState1>();

            flowManager.RegisterTransition(state1, () => test3 ? state3 : state2);
        }

        [ContextMenu("State1")]
        public void State1() => flowManager.Change(state1);

        [ContextMenu("State2")]
        public void State2() => flowManager.Change(state2);
    }
}
