using System;
using System.Collections.Generic;

namespace mm
{
    public class FiniteStateMachine
    {
        private Dictionary<Type, IState> stateDict;
        private Dictionary<Type, Func<Type>> transitionDict;
        private IState current;

        public FiniteStateMachine()
        {
            stateDict = new Dictionary<Type, IState>();
            transitionDict = new Dictionary<Type, Func<Type>>();
        }

        public IDictionary<Type, IState> States => stateDict;

        public IDictionary<Type, Func<Type>> Transition => transitionDict;

        public void Start<T>()
        {
            current?.OnStateEnd();
            current = GetState(typeof(T));
            current?.OnStateEnter();
        }

        public void Next()
        {
            current?.OnStateEnd();
            current = GetState(GetNext(current.GetType()));
            current?.OnStateEnter();
        }

        public void Update(double deltaTime)
        {
            if (current == null)
            {
                return;
            }
            else
            {
                current.StateUpdate(deltaTime);
                if (current.IsCompleted)
                {
                    current?.OnStateEnd();
                    current = GetState(GetNext(current.GetType()));
                    current?.OnStateEnter();
                }
            }
        }

        private Type GetNext(Type type)
        {
            if (type == null)
            {
                return default;
            }

            transitionDict.TryGetValue(type, out var next);
            return next == null ? default : next?.Invoke();
        }

        private IState GetState(Type type)
        {
            if (type == null)
            {
                return null;
            }

            stateDict.TryGetValue(type, out var next);
            return next;
        }
    }
}
