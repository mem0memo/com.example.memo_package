namespace mm.core.player
{
    public class TimeEternal : ITimeConverter
    {
        public double Convert(double time)
        {
            if (double.IsNaN(time))
            {
                return 0;
            }

            if (double.IsPositiveInfinity(time) || time >= double.MaxValue)
            {
                return double.MaxValue;
            }

            return time;
        }

        public void OnReset()
        {
        }
    }
}
