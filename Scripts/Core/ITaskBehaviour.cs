namespace mm.core
{
    public interface ITaskBehaviour
    {
        void OnStart();

        void OnEnd();

        void OnUpdate(TimeData time);
    }
}
