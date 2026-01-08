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

        private Dictionary<Type, IState> stateDict = new Dictionary<Type, IState>();
        private Dictionary<Type, Transition> transitionDict = new Dictionary<Type, Transition>();

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
            stateDict[typeof(TState)] = state;
            return state;
        }

        public void RegisterTransition<TState>(Func<Type> next)
        {
            transitionDict[typeof(TState)] = new Transition { Next = next };
        }

        public void Change<TState>()
            where TState : IState
        {
            if (stateDict.TryGetValue(typeof(TState), out var state))
            {
                taskRunner.Clear();
                stateMachine.Change(state);
                Log(state);
            }
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

            var type = current.GetType();
            if (transitionDict.TryGetValue(type, out var transition))
            {
                var nextType = transition.Next?.Invoke();
                if (nextType != null && stateDict.TryGetValue(nextType, out var next))
                {
                    stateMachine.Change(next);
                    Log(next);
                    return;
                }
            }

            stateMachine.Change(default);
            Log(null);
        }

        private void Log(IState state)
        {
            if (log)
            {
                var message = state == null ? "null" : state.GetType().Name;
                Debug.Log($"[State] : {message}");
            }
        }

        private struct Transition
        {
            public Func<Type> Next;
        }
    }
}
