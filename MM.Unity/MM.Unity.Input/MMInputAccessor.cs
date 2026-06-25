using System;
using System.Collections.Generic;
using mm.core.flow;
using UnityEngine;
using UnityEngine.InputSystem;

namespace mm.unity.input
{
    public class MMInputAccessor : MonoBehaviour, IInfrastructure
    {
        [SerializeField] private InputActionAsset _asset;
        [SerializeField] private ModeType mode = ModeType.Player;

        private Dictionary<PlayerActionType, InputAction> _player_map;
        private Dictionary<UIActionType, InputAction> _ui_map;
        private bool _didAwake;

        public enum ModeType { None, Player, UI }

        public enum PlayerActionType
        {
            Move, Look, Attack, Interact, Crouch, Jump, Previous, Next, Sprint,
        }

        public enum UIActionType
        {
            Navigate, Submit, Cancel, Point, Click, RightClick,
            MiddleClick, ScrollWheel, TrackedDevicePosition, TrackedDeviceOrientation,
        }

        private void Awake()
        {
            _player_map = BuildMap<PlayerActionType>(_asset.FindActionMap("Player", throwIfNotFound: true));
            _ui_map = BuildMap<UIActionType>(_asset.FindActionMap("UI", throwIfNotFound: true));
            _didAwake = true;
        }

        private void OnEnable() => UpdateEnable(mode);
        private void OnDisable() => UpdateEnable(ModeType.None);

        public void SetMode(ModeType newMode)
        {
            mode = newMode;
            if (_didAwake) UpdateEnable(mode);
        }

        public InputAction FindAction(PlayerActionType actionType) => _player_map[actionType];
        public InputAction FindAction(UIActionType actionType) => _ui_map[actionType];

        private Dictionary<T, InputAction> BuildMap<T>(InputActionMap map) where T : Enum
        {
            var dict = new Dictionary<T, InputAction>();
            foreach (var action in map.actions)
            {
                if (Enum.TryParse(typeof(T), action.name, out var parsed))
                    dict[(T)parsed] = action;
            }

            foreach (T t in Enum.GetValues(typeof(T)))
                Debug.AssertFormat(dict.ContainsKey(t), $"{typeof(T).Name}.{t} に対応するActionが asset 内に見つかりません（名前不一致の可能性）", this);

            return dict;
        }

        private void UpdateEnable(ModeType m)
        {
            foreach (var a in _player_map.Values) { if (m == ModeType.Player) a.Enable(); else a.Disable(); }
            foreach (var a in _ui_map.Values) { if (m == ModeType.UI) a.Enable(); else a.Disable(); }
        }
    }
}
