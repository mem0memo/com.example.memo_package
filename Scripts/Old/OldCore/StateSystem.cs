namespace mm.core
{
    public static class StateSystem
    {
        public interface IState
        {
            void OnEntry();
            void OnExit();
            void Update(TimeData timeData);
        }

        public interface IStateMachine : ServiceSystem.IService
        {
            void SetState(IState state);
        }

        public abstract class StateBase : IState
        {
            public virtual void OnExit()
            {
            }

            public virtual void OnEntry()
            {
            }

            public virtual void Update(TimeData timeData)
            {
            }
        }
    }
}