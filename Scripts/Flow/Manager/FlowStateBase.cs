using System;
using UnityEngine;

namespace mm
{
    public class FlowStateBase : MonoBehaviour, IState
    {
        public Action OnCompleted { get; set; }

        public void OnStateEnd()
        {
            OnStateEndImpl();
        }

        public void OnStateEnter()
        {
            OnStateEnterImpl();
        }

        public void StateUpdate(double deltaTime)
        {
            StateUpdateImpl(deltaTime);
        }

        protected void Complete() => OnCompleted?.Invoke();

        protected virtual void OnStateEnterImpl()
        {
        }

        protected virtual void OnStateEndImpl()
        {
        }

        protected virtual void StateUpdateImpl(double deltaTime)
        {
        }
    }
}
