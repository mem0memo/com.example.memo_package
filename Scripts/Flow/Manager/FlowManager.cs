using System;
using UnityEngine;

namespace mm.flow
{
    public class FlowManager : ServiceComponentBase, ITaskRunner
    {
        [SerializeField]
        private ServiceProvider serviceProvider;
        private TaskRunner taskRunner = new TaskRunner();
        private FiniteStateMachine stateMachine = new FiniteStateMachine();


        [SerializeField]
        private bool log;

        public TState RegisterState<TState>()
        where TState : FlowStateBase, new()
        {
            var stateContext = new FlowStateContext()
            {
                ServiceProvider = serviceProvider,
                TaskRunner = taskRunner,
            };

            var state = new TState();
            state.SetContext(stateContext);
            stateMachine.States[typeof(TState)] = state;
            return state;
        }

        public void RegisterTransition<TState>(Func<Type> next)
        {
            stateMachine.Transition[typeof(TState)] = next;
        }

        public void Change<TState>()
            where TState : IState
        {
            taskRunner.Clear();
            stateMachine.Start<TState>();
            Debug.Log($"[State] : {typeof(TState).Name}");
        }

        void ITaskRunner.End(ITask task) => taskRunner.End(task);

        void ITaskRunner.Run(ITask task) => taskRunner.Run(task);

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            stateMachine.Update(deltaTime);
            taskRunner.Update(deltaTime);
        }
    }
}
