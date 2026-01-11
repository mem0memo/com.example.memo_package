
namespace mm
{
    public interface IState
    {
        void OnStateEnter();

        void OnStateEnd();

        void StateUpdate(double deltaTime);
    }
}
