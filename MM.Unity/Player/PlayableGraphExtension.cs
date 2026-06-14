using mm.player;
using UnityEngine;
using UnityEngine.Playables;

namespace mm.unity.player
{
    public static class PlayableGraphExtension
    {
        public static CustomPlayer CreateOutputPlayer(
            this PlayableGraph graph, IPlayControl playControl, GameObject gameObject = default)
        {
            var playableOutput = ScriptPlayableOutput.Create(graph, "PlayableOutput");
            playableOutput.SetReferenceObject(gameObject);
            var playable = ScriptPlayable<PlayablePlayBehaviour>.Create(graph);
            playableOutput.SetSourcePlayable(playable);
            var customPlayer = new CustomPlayer(playControl);
            playable.GetBehaviour().SetControl(customPlayer);
            return customPlayer;
        }

        public static ScriptPlayableOutput CreateOutput(
            this PlayableGraph graph, string name, GameObject gameObject)
        {
            var output = ScriptPlayableOutput.Create(graph, name);
            output.SetReferenceObject(gameObject);
            return output;
        }
    }
}
