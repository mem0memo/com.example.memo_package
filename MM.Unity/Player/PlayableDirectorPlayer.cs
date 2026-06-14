using System;
using mm.player;
using UnityEngine;
using UnityEngine.Playables;

namespace mm.unity.player
{
    [ExecuteAlways]
    public class PlayableDirectorPlayer : MonoBehaviour, IPlayControl, IPlayer
    {
        [SerializeField]
        private PlayableDirector director;
        [SerializeField]
        private PlayModeType playMode;

        private CustomPlayer player;
        private PlayableGraph playableGraph;

        double IPlayer.Time => player.Time;

        bool IPlayer.IsPlaying => player.IsPlaying;

        public Action Ended { get; set; }

        public Action Looped { get; set; }


        public Action Played { get; set; }

        public Action Stopped { get; set; }

        public enum PlayModeType
        {
            Eternal,
            End,
            Loop,
        }

        [ContextMenu("Replay")]
        public void Replay()
        {
            player.Stop();
            player.Play();
        }

        [ContextMenu("Play")]
        public void Play()
        {
            player.Play();
        }

        [ContextMenu("Pause")]
        public void Pause()
        {
            player.Pause();
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            player.Stop();
        }

        public void SetPlayMode(PlayModeType playMode)
        {
            this.playMode = playMode;
            ITimeConverter converter = playMode switch
            {
                PlayModeType.Eternal => new TimeEternal(),
                PlayModeType.End => new TimeEnd(director.duration) { OnEnded = OnEnded },
                PlayModeType.Loop => new TimeLoop(director.duration) { OnLooped = OnLooped },
                _ => new TimeEternal(),
            };
            player?.SetTimeConverter(converter);
        }

        private void Awake()
        {
            playableGraph = PlayableGraph.Create();
            player = playableGraph.CreateOutputPlayer(this);
            director.timeUpdateMode = DirectorUpdateMode.Manual;
        }

        private void Start()
        {
            SetPlayMode(this.playMode);
        }

        private void OnEnable()
        {
            if (playableGraph.IsValid())
            {
                playableGraph.Play();
            }
        }

        private void OnDisable()
        {
            if (playableGraph.IsValid())
            {
                playableGraph.Stop();
            }
        }

        private void OnDestroy()
        {
            if (playableGraph.IsValid())
            {
                playableGraph.Destroy();
            }
        }

        private void OnValidate()
        {
            if (didAwake)
            {
                SetPlayMode(this.playMode);
            }
        }

        void IPlayControl.OnPlayEnd()
        {
            Stopped?.Invoke();
            director.Stop();
        }

        void IPlayControl.OnPlayStart()
        {
            director.timeUpdateMode = DirectorUpdateMode.Manual;
            director.Play();
            Played?.Invoke();
        }

        void IPlayControl.OnPlayTimeUpdate(double time)
        {
            if (director.time != time)
            {
                director.time = time;
                director.Evaluate();
            }
        }

        private void OnEnded()
        {
            Ended?.Invoke();
            Debug.Log("[director player] end");
        }

        private void OnLooped()
        {
            Looped?.Invoke();
            Debug.Log("[director player] loop");
        }
    }
}
