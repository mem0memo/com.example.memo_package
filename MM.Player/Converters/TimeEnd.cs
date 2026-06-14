using System;

namespace mm.player
{
    public class TimeEnd : ITimeConverter
    {
        private double duration;
        private bool isActioned;

        public TimeEnd(double duration)
        {
            this.duration = duration;
        }

        public Action OnEnded { get; set; }

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

            if (!isActioned && duration <= time)
            {
                OnEnded?.Invoke();
                isActioned = true;
            }

            return duration <= time ? duration : time;
        }

        public void OnReset()
        {
            isActioned = false;
        }
    }
}
