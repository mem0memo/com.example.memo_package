using System;
using System.Collections.Generic;
using UnityEngine;

namespace mm.flow
{
    public class FlowManager : ServiceComponentBase, ITaskRunner
    {
        [SerializeField]
        private ServiceProvider serviceProvider;
        private TaskRunner taskRunner = new TaskRunner();
        private StateMachine stateMachine = new StateMachine();
        private Dictionary<IState, Transition> transitionDict = new Dictionary<IState, Transition>();

        public TState CreateState<TState>()
        where TState : FlowStateBase, new()
        {
            var stateContext = new FlowStateContext()
            {
                ServiceProvider = serviceProvider,
                TaskRunner = taskRunner,
            };

            var state = new TState();
            state.SetContext(stateContext);
            return state;
        }

        public void RegisterTransition(IState state, Func<IState> next)
        {
            transitionDict[state] = new Transition { Next = next };
        }

        public void Change<TState>(TState state)
            where TState : IState
        {
            taskRunner.Clear();
            stateMachine.Change(state);
        }

        void ITaskRunner.End(ITask task) => taskRunner.End(task);

        void ITaskRunner.Run(ITask task) => taskRunner.Run(task);

        private void Awake()
        {
            stateMachine.Completed += Next;
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            stateMachine.Update(deltaTime);
            taskRunner.Update(deltaTime);
        }

        private void OnDestroy()
        {
            if (stateMachine != null)
            {
                stateMachine.Completed -= Next;
            }
        }

        private void Next(IState current)
        {
            if (current == null)
            {
                return;
            }

            if (transitionDict.TryGetValue(current, out var transition))
            {
                var next = transition.Next?.Invoke();
                stateMachine.Change(next);
            }
        }

        private struct Transition
        {
            public Func<IState> Next;
        }
    }
}
