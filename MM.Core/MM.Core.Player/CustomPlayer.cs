namespace mm.core.player
{
    public class CustomPlayer : PlayerBase
    {
        private IPlayControl playControl;
        private ITimeConverter timeConverter;
        public CustomPlayer(IPlayControl playControl)
        {
            this.playControl = playControl;
            this.timeConverter = new TimeEternal();
        }

        public void SetTimeConverter(ITimeConverter converter)
        {
            this.timeConverter = converter;
        }

        protected override void OnPlayStart()
        {
            timeConverter?.OnReset();
            playControl.OnPlayStart();
        }

        protected override void OnPlayStop()
        {
            playControl.OnPlayEnd();
        }

        protected override void OnPlayTimeUpdate(double time)
        {
            playControl.OnPlayTimeUpdate(time);
        }

        protected override double TimeConvert(double time)
        => timeConverter == null ? 0 : timeConverter.Convert(time);
    }
}
