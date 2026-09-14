using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace mm.unity
{

    /// <summary>
    /// MM_input(Playerマップ)の入力をログに出すデバッガ。
    /// アクションを列挙して判定するので、アクションを追加・改名してもコード変更は不要。
    /// </summary>
    public class MM_inputDebugger : MonoBehaviour
    {
        [SerializeField] private bool logVector2 = true;
        [SerializeField] private bool logButton = true;

        private MM_input _input;
        private readonly Dictionary<InputAction, Vector2> _lastVector2 = new();

        // 生成クラスはインスタンス生成 → Enable → Disable → Dispose の順で扱う
        // (Disable し忘れるとデストラクタのAssertが出る)
        private void Awake()
        {
            _input = new MM_input();
        }

        private void OnEnable()
        {
            _lastVector2.Clear();
            _input.Player.Enable();
        }

        private void OnDisable()
        {
            _input.Player.Disable();
        }

        private void OnDestroy()
        {
            _input.Dispose();
        }

        private void Update()
        {
            foreach (var action in _input.Player.Get().actions)
            {
                if (action.expectedControlType == "Vector2")
                {
                    if (!logVector2) continue;

                    // 連続値は毎フレーム読める(pull)。値が変わったときだけログに出す
                    var value = action.ReadValue<Vector2>();
                    _lastVector2.TryGetValue(action, out var last);
                    if (value == last) continue;

                    _lastVector2[action] = value;
                    Debug.Log($"[MM_input] {action.name}: {value}", this);
                    continue;
                }

                if (!logButton) continue;

                // ボタンは押した/離したフレームだけ true になる(pull)
                if (action.WasPressedThisFrame())
                {
                    var device = action.activeControl?.device.displayName;
                    Debug.Log($"[MM_input] {action.name}: Pressed ({device})", this);
                }

                if (action.WasReleasedThisFrame())
                {
                    Debug.Log($"[MM_input] {action.name}: Released", this);
                }
            }
        }
    }
}
