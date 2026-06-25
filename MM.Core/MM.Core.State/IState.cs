namespace mm.core.state
{
    public interface IState
    {
        void Start();
        void Stop();
        void Update(double deltaTime);
    }
}