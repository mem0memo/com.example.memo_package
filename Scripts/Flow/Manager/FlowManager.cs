using System;
using UnityEngine;

namespace mm.flow
{
    public class FlowManager : MonoBehaviour
    {
        [SerializeField]
        private ServiceProvider serviceProvider;

        private TaskRunner taskRunner;
        private StateMachine stateMachine;

        public TState CreateState<TState>()
            where TState : StateBase
        {
            var args = new StateBase.Context
            {
                ServiceProvider = serviceProvider,
                TaskRunner = taskRunner,
            };

            return (TState)Activator.CreateInstance(typeof(TState), args);
        }

        public void ChangeState(IState state) => stateMachine.Set(state);

        private void Awake()
        {
            taskRunner = new TaskRunner();
            stateMachine = new StateMachine();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            stateMachine.Update(deltaTime);
            taskRunner.Update(deltaTime);
        }
    }
}
