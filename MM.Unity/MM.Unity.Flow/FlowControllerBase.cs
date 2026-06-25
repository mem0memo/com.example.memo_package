using System.Collections.Generic;
using mm.core.flow;
using mm.core.state;
using UnityEngine;

namespace mm.unity.flow
{
    public abstract class FlowControllerBase<TFlowType, TModel> :
    MonoBehaviour,
    IFlowController<TFlowType>,
    IModelProvider<TModel>
    {
        [SerializeField]
        private InfraProvider infraProvider;
        private StateMachine stateMachine;
        private Dictionary<TFlowType, IState> flowMap;
        [SerializeField]
        private TFlowType flowType;

        TFlowType IFlowController<TFlowType>.CurrentFlow => flowType;

        protected abstract TFlowType[] FlowTypes { get; }

        TModel IModelProvider<TModel>.GetModel() => GetModel();

        void IFlowController<TFlowType>.StartFlow(TFlowType type)
        {
            flowType = type;
            stateMachine.Start(GetFlow(type));
        }

        protected abstract TModel GetModel();

        protected abstract IState CreateFlow(
            TFlowType flowType, FlowContext flowContext, InfraProvider infraProvider);

        private void Awake()
        {
            flowMap = new Dictionary<TFlowType, IState>();
            stateMachine = new StateMachine();
        }

        private void Start()
        {
            var flowContext = new FlowContext
            {
                FlowController = infraProvider.Find<IFlowController<TFlowType>>(),
                ModelProvider = infraProvider.Find<IModelProvider<TModel>>(),
            };

            foreach (var flowType in FlowTypes)
            {
                var flow = CreateFlow(flowType, flowContext, infraProvider);
                flowMap[flowType] = flow;
            }

            stateMachine.Start(GetFlow(flowType));
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            stateMachine.Update(deltaTime);
        }

        private void OnValidate()
        {
            if (didAwake)
            {
                stateMachine.Start(GetFlow(flowType));
            }
        }

        private IState GetFlow(TFlowType flowType)
        => flowMap.TryGetValue(flowType, out var flow) ? flow : default;

        protected struct FlowContext
        {
            public IFlowController<TFlowType> FlowController;
            public IModelProvider<TModel> ModelProvider;
        }
    }
}
