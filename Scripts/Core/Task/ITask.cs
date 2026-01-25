namespace mm
{
    public interface ITask
    {
        void OnEnter();
        void OnRemove();
        void Tick(double deltaTime);
    }
}
