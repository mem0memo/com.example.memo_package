namespace mm.player
{
    public interface IPlayer
    {
        public double Time { get; }

        public bool IsPlaying { get; }

        void Play();
        void Pause();
        void Stop();

        void Replay()
        {
            Stop();
            Play();
        }
    }
}
