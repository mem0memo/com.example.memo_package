using System.Collections.Generic;
using mm.core.flow;
using mm.core.state;
using UnityEngine;

namespace mm.unity.flow
{
    public abstract class FlowManagerBase<TFlowType> : MonoBehaviour, IFlowController<TFlowType>
    {
        [SerializeField]
        private InfraProvider infraProvider;
        [SerializeField]
        private TFlowType flowType;

        private StateMachine stateMachine;
        private Dictionary<TFlowType, IState> flowMap;

        TFlowType IFlowController<TFlowType>.CurrentFlow => flowType;

        void IFlowController<TFlowType>.StartFlow(TFlowType type) => StartFlow(type);

        protected void StartFlow(TFlowType type)
        {
            flowType = type;
            flowMap.TryGetValue(flowType, out var flow);
            stateMachine.Start(flow);
        }

        protected abstract TFlowType[] FlowTypes { get; }

        protected abstract IState CreateFlow(TFlowType flowType, InfraProvider infraProvider);

        protected virtual IEnumerable<IInfrastructure> GetStartupInfrastructures()
        {
            return new IInfrastructure[] { };
        }

        private void Awake()
        {
            flowMap = new Dictionary<TFlowType, IState>();
            stateMachine = new StateMachine();
        }

        private void Start()
        {
            // infra
            infraProvider.Register(this);
            var startupInfrastructures = GetStartupInfrastructures();
            foreach (var infra in startupInfrastructures)
            {
                infraProvider.Register(infra);
            }

            // setup flows
            foreach (var flowType in FlowTypes)
            {
                var flow = CreateFlow(flowType, infraProvider);
                flowMap[flowType] = flow;
            }

            // start
            StartFlow(flowType);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            stateMachine.Update(deltaTime);
        }

        [ContextMenu("Restart Flow")]
        private void RestartFlow()
        {
            StartFlow(flowType);
        }

        protected class StateBase<TController> : CtsStateBase
        {
            public StateBase(InfraProvider infraProvider, TController controller)
            {
                this.Infras = infraProvider;
                this.Controller = controller;
                this.Flow = infraProvider.Find<IFlowController<TFlowType>>();
            }

            protected TController Controller { get; private set; }

            protected InfraProvider Infras { get; private set; }

            protected IFlowController<TFlowType> Flow { get; private set; }
        }
    }
}
