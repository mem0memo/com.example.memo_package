using System.Collections.Generic;
using UnityEngine;

namespace mm.flow
{
    public class FlowController : MonoBehaviour, IStateService
    {
        private StateMachine stateMachine;
        private Dictionary<int, IState> stateDict;

        public void Register(int key, IState state) => stateDict[key] = state;

        public void Set(int key) => stateMachine.Set(stateDict[key]);

        private void Awake()
        {
            stateMachine = new StateMachine();
            stateDict = new Dictionary<int, IState>();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            stateMachine.Update(deltaTime);
        }
    }
}
