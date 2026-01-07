
namespace mm
{
    public interface IState
    {
        bool IsCompleted { get; }

        void OnStateEnter();

        void OnStateEnd();

        void StateUpdate(double deltaTime);
    }
}
