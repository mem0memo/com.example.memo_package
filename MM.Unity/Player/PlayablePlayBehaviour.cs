using mm.player;
using UnityEngine.Playables;

namespace mm.unity.player
{
    public class PlayablePlayBehaviour : PlayableBehaviour
    {
        private PlayerBase player;

        public void SetControl(PlayerBase player)
        {
            this.player = player;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            var deltaTime = info.deltaTime;
            player.UpdateTime(deltaTime);
        }
    }
}
