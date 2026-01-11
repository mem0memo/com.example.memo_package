using System;
using System.Collections.Generic;
using UnityEngine;

namespace mm.flow
{
    public class FlowManager : ServiceComponentBase, ITaskRunner
    {
        private TaskRunner taskRunner;
        private StateMachine stateMachine;

        public void SetStateTable(IStateTable stateTable)
        {
            this.stateMachine = new StateMachine(stateTable);
        }

        public void End(ITask task) => taskRunner.End(task);

        public void Run(ITask task) => taskRunner.Run(task);

        public void SetState(IState state) => stateMachine.Set(state);

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
