using System;

namespace mm.core.player
{
    public class TimeLoop : ITimeConverter
    {
        private double duration;
        private double prevTime;

        public TimeLoop(double duration)
        {
            this.duration = duration;
        }

        public Action OnLooped { get; set; }

        public double Convert(double time)
        {
            if (double.IsNaN(time))
            {
                return 0;
            }

            if (duration <= 0 || double.IsNaN(duration) || double.IsInfinity(duration))
            {
                return 0;
            }

            var convertedTime = time % duration;
            if (convertedTime < prevTime)
            {
                OnLooped?.Invoke();
            }

            prevTime = convertedTime;
            return convertedTime;
        }

        public void OnReset()
        {
            this.prevTime = 0;
        }
    }
}
