using UnityEngine;

namespace mm.flow
{
    public class Test : MonoBehaviour
    {
        private TestState1 state1;
        private TestState2 state2;

        [SerializeField]
        private FlowManager flowManager;

        private void Awake()
        {
            state1 = flowManager.CreateState<TestState1>();
            state2 = flowManager.CreateState<TestState2>();
        }


        [ContextMenu("State1")]
        public void State1() => flowManager.ChangeState(state1);

        [ContextMenu("State2")]
        public void State2() => flowManager.ChangeState(state2);
    }
}
