using System;
using UnityEngine;

namespace mm.unity.input
{
    public interface IMMInput
    {
        public enum KeyType
        {
            option, // Start, Menu. マウス右
            Select,
            TouchPad,

            Confirm, // ☓, マウス左
            Cancel, // ○
            Attack, // □
            Detail, // △

            Up, // 矢印キー, Stick等
            Down, // 矢印キー, Stick等
            Left, // 矢印キー, Stick等
            Right, // 矢印キー, Stick等

            L1,
            R1,
            L2,
            R2,
            L3, //押し込み
            R3, //押し込み
        }

        public enum AxisType
        {
            Move, // LStick, 矢印キーなど
            Look, //  RStickなどのベクトル値
        }

        public enum TriggerType
        {
            L2,
            R2,
        }

        public enum EventType
        {
            Down,
            Up,
        }

        public Action<FrameInfo> FrameEvent { get; set; }

        /// <summary>
        /// このフレームでの入力
        /// </summary>
        bool Frame(KeyType key, EventType eventType);

        bool Pressed(KeyType key, out float duration);

        /// <summary>
        /// LStick, RStickなどのベクトル値
        /// </summary>
        Vector2 Axis(AxisType axis);

        /// <summary>
        /// L2, R2などのアナログ値
        /// </summary>
        float Trigger(TriggerType trigger);

        public struct FrameInfo
        {
            public KeyType Key;
            public EventType Event;

            public static FrameInfo Down(KeyType key) => new FrameInfo
            {
                Key = key,
                Event = EventType.Down,
            };

            public static FrameInfo Up(KeyType key) => new FrameInfo
            {
                Key = key,
                Event = EventType.Up,
            };
        }
    }
}
