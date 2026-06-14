namespace mm.player
{
    public static class CustomPlayerExtension
    {
        public static TimeLoop LoopMode(this CustomPlayer player, double duration)
        {
            var loop = new TimeLoop(duration);
            player.SetTimeConverter(loop);
            return loop;
        }

        public static void EternalMode(this CustomPlayer player)
        {
            var converter = new TimeEternal();
            player.SetTimeConverter(converter);
        }

        public static void EndMode(this CustomPlayer player, double duration)
        {
            var converter = new TimeEnd(duration);
            player.SetTimeConverter(converter);
        }
    }
}
