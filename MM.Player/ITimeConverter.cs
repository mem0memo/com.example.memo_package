namespace mm.player
{
    public interface ITimeConverter
    {
        double Convert(double time);

        void OnReset();
    }
}
