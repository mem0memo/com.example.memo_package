using System;
using System.Collections.Generic;
using UnityEngine;

namespace mm.unity.input
{
    public sealed class MMInputLogger : MonoBehaviour
    {
        [SerializeField] private MMInputBase source;
        [SerializeField] private bool logUpDown = true;
        [SerializeField] private bool logIntervalHold = true;
        [SerializeField] private bool logLongHold = true;
        [SerializeField] private bool logValues = true;

        private static readonly IMMInput.KeyType[] Keys =
            (IMMInput.KeyType[])Enum.GetValues(
                typeof(IMMInput.KeyType));

        private static readonly IMMInput.AxisType[] Axises =
            (IMMInput.AxisType[])Enum.GetValues(
            typeof(IMMInput.AxisType));

        private static readonly IMMInput.TriggerType[] Triggers =
            (IMMInput.TriggerType[])Enum.GetValues(
                typeof(IMMInput.TriggerType));

        private readonly Dictionary<IMMInput.AxisType, Vector2>
            lastVectors = new();

        private readonly Dictionary<IMMInput.TriggerType, float>
            lastValues = new();

        private MMInputBase subscribedSource;

        private void OnEnable()
        {
            lastVectors.Clear();
            lastValues.Clear();

            subscribedSource = source;

            if (subscribedSource != null)
                subscribedSource.FrameEvent += Log;
        }

        private void OnDisable()
        {
            if (subscribedSource != null)
                subscribedSource.FrameEvent -= Log;

            subscribedSource = null;
        }

        private void Log(IMMInput.FrameInfo inputEvent)
        {
            if (!logUpDown &&
                (inputEvent.Event == IMMInput.EventType.Up || inputEvent.Event == IMMInput.EventType.Down))
                return;

            Debug.Log(
                $"[{Time.time:F3}]" +
                $"{inputEvent.Key} / {inputEvent.Event} ",
                subscribedSource);
        }

        // 入力実装の Update が終わった後に状態を確認する。
        private void LateUpdate()
        {
            if (source == null)
                return;

            // axis
            foreach (var type in Axises)
            {
                Vector2 vector = source.Axis(type);
                lastVectors.TryGetValue(type, out Vector2 previousVector);
                bool changed = !vector.Equals(previousVector);
                lastVectors[type] = vector;

                if (!logValues || !changed)
                    continue;

                Debug.Log(
                    $"[{Time.time:F3}] {type} / 値変化 " +
                    $"Vector={vector.ToString("F3")}",
                    source);
            }

            // trigger
            foreach (var type in Triggers)
            {
                float value = source.Trigger(type);
                lastValues.TryGetValue(type, out float previousValue);
                bool changed = !value.Equals(previousValue);
                lastValues[type] = value;

                if (!logValues || !changed)
                    continue;

                Debug.Log(
                    $"[{Time.time:F3}] {type} / 値変化 " +
                    $"Value={value:F3}",
                    source);
            }
        }
    }
}