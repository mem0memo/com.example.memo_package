using System;
using System.Collections.Generic;
using UnityEngine;

namespace mm.flow
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private FlowManager flowManager;
        [SerializeField]
        private ServiceProvider serviceProvider;

        private FiniteStateTable stateTable;
        private Dictionary<Type, IState> stateDict;

        private void Awake()
        {
            stateTable = new FiniteStateTable();
            stateDict = new Dictionary<Type, IState>();
            stateDict[typeof(TestState1)] = new TestState1(serviceProvider);
            stateDict[typeof(TestState2)] = new TestState2(serviceProvider);
        }

        private void Start()
        {
            stateTable.Transition.Add(typeof(TestState1), () => typeof(TestState2));
        }

        [ContextMenu("State1")]
        public void State1() => flowManager.SetState(stateDict[typeof(TestState1)]);

        [ContextMenu("State2")]
        public void State2() => flowManager.SetState(stateDict[typeof(TestState2)]);
    }
}
