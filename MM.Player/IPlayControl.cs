namespace mm.player
{
    public interface IPlayControl
    {
        void OnPlayStart();

        void OnPlayEnd();

        void OnPlayTimeUpdate(double time);
    }
}
