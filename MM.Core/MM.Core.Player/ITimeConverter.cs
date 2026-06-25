namespace mm.core.player
{
    public interface ITimeConverter
    {
        double Convert(double time);

        void OnReset();
    }
}
