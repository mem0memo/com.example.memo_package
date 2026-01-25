
namespace mm
{
    public interface IState
    {
        void OnEnter();

        void OnQuit();

        void Tick(double deltaTime);
    }
}
