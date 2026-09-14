namespace mm.core.state
{
    public interface IState
    {
        void Enter();
        void Exit();
        void ProcessFrame(double deltaTime);
    }
}