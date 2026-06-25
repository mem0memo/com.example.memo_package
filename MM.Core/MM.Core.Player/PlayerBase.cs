namespace mm.core.player
{
    /// <summary>
    /// Player
    /// </summary>
    public partial class PlayerBase : IPlayer
    {
        private double time;
        private bool isPlaying;

        public PlayerBase()
        {
            time = 0;
            isPlaying = false;
        }

        public double Time => time;

        public bool IsPlaying => isPlaying;

        public void Play()
        {
            isPlaying = true;
            OnPlayStart();
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public void Stop()
        {
            time = 0;
            isPlaying = false;
            OnPlayStop();
        }

        public void UpdateTime(float deltaTime)
        {
            if (isPlaying)
            {
                var nextTime = time + deltaTime;
                time = double.IsNaN(nextTime) || nextTime >= double.MaxValue
                    ? double.MaxValue
                    : nextTime;
                time = TimeConvert(time);
                OnPlayTimeUpdate(time);
            }
        }

        protected virtual void OnPlayStart()
        {
        }

        protected virtual void OnPlayStop()
        {
        }

        protected virtual void OnPlayTimeUpdate(double time)
        {
        }

        protected virtual double TimeConvert(double time) => time;
    }

    /// <summary>
    /// Control
    /// </summary>
    public partial class PlayerBase : IPlayControl
    {
        void IPlayControl.OnPlayStart()
        {
            isPlaying = false;
            OnPlayStart();
        }

        void IPlayControl.OnPlayEnd()
        {
            isPlaying = false;
            OnPlayStop();
        }

        void IPlayControl.OnPlayTimeUpdate(double time)
        {
            this.time = time;
            OnPlayTimeUpdate(time);
        }
    }
}