using System;
using System.Collections.Generic;
using UnityEngine;
using static mm.unity.input.IMMInput;

namespace mm.unity.input
{
    public abstract class MMInputBase : MonoBehaviour, IMMInput
    {
        private Dictionary<KeyType, KeyState> stateDict = new Dictionary<KeyType, KeyState>()
        {
            {KeyType.option, new KeyState()},
            {KeyType.Select, new KeyState()},
            {KeyType.TouchPad, new KeyState()},
            {KeyType.Confirm, new KeyState()},
            {KeyType.Cancel, new KeyState()},
            {KeyType.Attack, new KeyState()},
            {KeyType.Detail, new KeyState()},
            {KeyType.Up, new KeyState()},
            {KeyType.Down, new KeyState()},
            {KeyType.Left, new KeyState()},
            {KeyType.Right, new KeyState()},
            {KeyType.L1, new KeyState()},
            {KeyType.R1, new KeyState()},
            {KeyType.L2, new KeyState()},
            {KeyType.R2, new KeyState()},
            {KeyType.L3, new KeyState()},
            {KeyType.R3, new KeyState()},
        };

        public Action<FrameInfo> FrameEvent { get; set; }

        public bool Frame(KeyType key, IMMInput.EventType eventType)
        {
            switch (eventType)
            {
                case IMMInput.EventType.Down:
                    return WasPressedThisFrame(key);
                case IMMInput.EventType.Up:
                    return WasReleasedThisFrame(key);
                default:
                    return false;
            }
        }

        public bool Pressed(KeyType key, out float duration)
        {
            if (IsPressed(key) && stateDict.TryGetValue(key, out var state))
            {
                duration = Time.time - state.StartTime;
                return true;
            }

            duration = 0;
            return false;
        }

        public Vector2 Axis(AxisType axis)
            => GetAxis(axis);

        public float Trigger(TriggerType trigger)
            => GetTrigger(trigger);

        protected abstract float GetTrigger(TriggerType triggerType);

        protected abstract Vector2 GetAxis(AxisType axisType);

        protected abstract bool IsPressed(KeyType keyType);

        protected abstract bool WasPressedThisFrame(KeyType keyType);

        protected abstract bool WasReleasedThisFrame(KeyType keyType);

        protected void SendEvent(KeyType key, IMMInput.EventType eventType)
        {
            var frameInfo = new FrameInfo
            {
                Key = key,
                Event = eventType,
            };

            stateDict[key] = eventType switch
            {
                IMMInput.EventType.Down => new KeyState { StartTime = Time.time },
                IMMInput.EventType.Up => new KeyState { StartTime = -1 },
                _ => default,
            };

            FrameEvent?.Invoke(frameInfo);
        }

        private struct KeyState
        {
            public float StartTime;
        }
    }
}
