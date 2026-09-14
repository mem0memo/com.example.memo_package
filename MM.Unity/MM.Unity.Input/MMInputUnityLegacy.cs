using System;
using System.Collections.Generic;
using UnityEngine;
using mm.unity.attribute;
using static mm.unity.input.IMMInput;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

namespace mm.unity.input
{
    /// <summary>
    /// IMMInput / MMInputBase 用の Legacy Input 実装。
    /// コントローラーは従来の PS4 純正・Windows USB・接続順1台目の軸配置を使用する。
    /// </summary>
    [DefaultExecutionOrder(-200)]
    public sealed class MMInputUnityLegacy : MMInputBase
    {
        [Serializable]
        private sealed class KeyBinding
        {
            public KeyType key;
            [KeySelect] public KeyCode[] keys = Array.Empty<KeyCode>();
        }

        [Header("Keys")]
        [SerializeField] private KeyBinding[] bindings = CreateDefaultBindings();

        [Header("Axis / Trigger")]
        [Tooltip("方向キーの割り当てを Move のベクトルにも反映します。")]
        [SerializeField] private bool keyboardMove = true;
        [Tooltip("左スティックから Up / Down / Left / Right の押下も判定します。")]
        [SerializeField] private bool stickDirectionKeys = true;
        [SerializeField, Range(0.01f, 1f)] private float directionThreshold = 0.5f;
        [SerializeField, Range(0.01f, 1f)] private float triggerThreshold = 0.5f;

        private static readonly KeyType[] Keys = (KeyType[])Enum.GetValues(typeof(KeyType));

        // Down / Up の変化検出に必要な履歴だけ。軸値・トリガー値は保存しない。
        private readonly Dictionary<KeyType, bool> previousPresses = new();

        private const string MoveX = "MMInput_PS4_USB_J1_MoveX";
        private const string MoveY = "MMInput_PS4_USB_J1_MoveY";
        private const string LookX = "MMInput_PS4_USB_J1_LookX";
        private const string LookY = "MMInput_PS4_USB_J1_LookY";
        private const string L2Axis = "MMInput_PS4_USB_J1_L2";
        private const string R2Axis = "MMInput_PS4_USB_J1_R2";
        private const string DirectionX = "MMInput_PS4_USB_J1_DirectionX";
        private const string DirectionY = "MMInput_PS4_USB_J1_DirectionY";

        public Vector2 DirectionPad => isActiveAndEnabled && IsConnected()
            ? ReadVector(DirectionX, DirectionY)
            : Vector2.zero;

        private void OnEnable() => previousPresses.Clear();

        private void Update()
        {
            foreach (var key in Keys)
            {
                if (!isActiveAndEnabled)
                    break;

                bool pressed = IsPressed(key);
                previousPresses.TryGetValue(key, out bool previous);
                previousPresses[key] = pressed;

                // 複数の割り当てを OR 合成した後に変化を判定する。
                // 別のキーがまだ押されているなら Up を出さない。
                if (pressed == previous)
                    continue;
                if (pressed)
                    SendEvent(key, IMMInput.EventType.Down);
                else
                    SendEvent(key, IMMInput.EventType.Up);
            }
        }

        private void OnDisable()
        {
            foreach (var key in Keys)
            {
                if (!previousPresses.TryGetValue(key, out bool pressed) || !pressed)
                    continue;
                previousPresses[key] = false;
                SendEvent(key, IMMInput.EventType.Up);
            }
            previousPresses.Clear();
        }

        protected override bool IsPressed(KeyType keyType)
        {
            if (!isActiveAndEnabled)
                return false;
            if (AnyKey(keyType))
                return true;

            switch (keyType)
            {
                case KeyType.L2:
                    return GetTrigger(TriggerType.L2) >= Mathf.Clamp(triggerThreshold, 0.01f, 1f);
                case KeyType.R2:
                    return GetTrigger(TriggerType.R2) >= Mathf.Clamp(triggerThreshold, 0.01f, 1f);
                case KeyType.Up:
                case KeyType.Down:
                case KeyType.Left:
                case KeyType.Right:
                    if (!IsConnected())
                        return false;
                    float threshold = Mathf.Clamp(directionThreshold, 0.01f, 1f);
                    return DirectionPressed(keyType, ReadVector(DirectionX, DirectionY), threshold) ||
                        (stickDirectionKeys && DirectionPressed(keyType, ReadVector(MoveX, MoveY), threshold));
                default:
                    return false;
            }
        }

        protected override Vector2 GetAxis(AxisType axisType)
        {
            if (!isActiveAndEnabled)
                return Vector2.zero;

            switch (axisType)
            {
                case AxisType.Move:
                    Vector2 move = IsConnected() ? ReadVector(MoveX, MoveY) : Vector2.zero;
                    if (keyboardMove)
                    {
                        // IsPressed を経由しない。スティックとの二重合成を防ぐ。
                        move += new Vector2(
                            (AnyKey(KeyType.Right) ? 1f : 0f) - (AnyKey(KeyType.Left) ? 1f : 0f),
                            (AnyKey(KeyType.Up) ? 1f : 0f) - (AnyKey(KeyType.Down) ? 1f : 0f));
                    }
                    return Vector2.ClampMagnitude(move, 1f);

                case AxisType.Look:
                    return IsConnected()
                        ? Vector2.ClampMagnitude(ReadVector(LookX, LookY), 1f)
                        : Vector2.zero;

                default:
                    return Vector2.zero;
            }
        }

        protected override float GetTrigger(TriggerType triggerType)
        {
            // 未接続時の生値 0 を 0.5 に変換しない。
            if (!isActiveAndEnabled || !IsConnected())
                return 0f;

            return triggerType switch
            {
                TriggerType.L2 => ReadTrigger(L2Axis),
                TriggerType.R2 => ReadTrigger(R2Axis),
                _ => 0f,
            };
        }

        protected override bool WasPressedThisFrame(KeyType keyType)
        {
            foreach (var binding in bindings ?? Array.Empty<KeyBinding>())
            {
                if (binding == null || binding.key != keyType || binding.keys == null)
                    continue;
                foreach (var key in binding.keys)
                {
                    if (key != KeyCode.None && Input.GetKeyDown(key))
                        return true;
                }
            }
            return false;
        }

        protected override bool WasReleasedThisFrame(KeyType keyType)
        {
            foreach (var binding in bindings ?? Array.Empty<KeyBinding>())
            {
                if (binding == null || binding.key != keyType || binding.keys == null)
                    continue;
                foreach (var key in binding.keys)
                {
                    if (key != KeyCode.None && Input.GetKeyUp(key))
                        return true;
                }
            }
            return false;
        }

        private bool AnyKey(KeyType keyType)
        {
            foreach (var binding in bindings ?? Array.Empty<KeyBinding>())
            {
                if (binding == null || binding.key != keyType || binding.keys == null)
                    continue;
                foreach (var key in binding.keys)
                {
                    if (key != KeyCode.None && Input.GetKey(key))
                        return true;
                }
            }
            return false;
        }

        private static bool DirectionPressed(KeyType key, Vector2 vector, float threshold)
        {
            return key switch
            {
                KeyType.Up => vector.y >= threshold,
                KeyType.Down => vector.y <= -threshold,
                KeyType.Left => vector.x <= -threshold,
                KeyType.Right => vector.x >= threshold,
                _ => false,
            };
        }

        private static bool IsConnected()
        {
            string[] names = Input.GetJoystickNames();
            return names.Length > 0 && !string.IsNullOrEmpty(names[0]);
        }

        private static Vector2 ReadVector(string x, string y) =>
            new Vector2(Input.GetAxisRaw(x), Input.GetAxisRaw(y));

        private static float ReadTrigger(string axis) =>
            Mathf.Clamp01((Input.GetAxisRaw(axis) + 1f) * 0.5f);

        private static KeyBinding Key(KeyType key, params KeyCode[] keys) =>
            new KeyBinding { key = key, keys = keys };

        private static KeyBinding[] CreateDefaultBindings() => new[]
        {
            Key(KeyType.option, KeyCode.Joystick1Button9, KeyCode.Escape, KeyCode.Mouse1),
            Key(KeyType.Select, KeyCode.Joystick1Button8, KeyCode.Tab),
            Key(KeyType.TouchPad, KeyCode.Joystick1Button13),
            // 最新の IMMInput に合わせて Confirm=×、Cancel=○。
            Key(KeyType.Confirm, KeyCode.Joystick1Button1, KeyCode.Return, KeyCode.Mouse0),
            Key(KeyType.Cancel, KeyCode.Joystick1Button2, KeyCode.Backspace),
            Key(KeyType.Attack, KeyCode.Joystick1Button0, KeyCode.F),
            Key(KeyType.Detail, KeyCode.Joystick1Button3, KeyCode.E),
            Key(KeyType.Up, KeyCode.UpArrow, KeyCode.W),
            Key(KeyType.Down, KeyCode.DownArrow, KeyCode.S),
            Key(KeyType.Left, KeyCode.LeftArrow, KeyCode.A),
            Key(KeyType.Right, KeyCode.RightArrow, KeyCode.D),
            Key(KeyType.L1, KeyCode.Joystick1Button4, KeyCode.Q),
            Key(KeyType.R1, KeyCode.Joystick1Button5, KeyCode.R),
            // L2/R2 の押下はアナログ軸と triggerThreshold で判定する。
            // 追加でキーボード等を割り当てたい場合だけキー欄を使う。
            Key(KeyType.L2),
            Key(KeyType.R2),
            Key(KeyType.L3, KeyCode.Joystick1Button10, KeyCode.LeftShift),
            Key(KeyType.R3, KeyCode.Joystick1Button11),
        };

#if UNITY_EDITOR
        private void Reset() => bindings = CreateDefaultBindings();

        [ContextMenu("初期割り当てを追加（不足分のみ）")]
        public void AddDefaultBindings()
        {
            if (EditorApplication.isPlaying)
                return;
            var merged = new List<KeyBinding>(bindings ?? Array.Empty<KeyBinding>());
            var assigned = new HashSet<KeyType>();
            foreach (var binding in merged)
            {
                if (binding != null)
                    assigned.Add(binding.key);
            }
            bool changed = false;
            foreach (var binding in CreateDefaultBindings())
            {
                if (assigned.Add(binding.key))
                {
                    merged.Add(binding);
                    changed = true;
                }
            }
            if (!changed)
                return;

            Undo.RecordObject(this, "MMInput の初期割り当てを追加");
            bindings = merged.ToArray();
            if (PrefabUtility.IsPartOfPrefabInstance(this))
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}

#if UNITY_EDITOR
namespace mm.unity.input.editor
{
    [CustomEditor(typeof(MMInputUnityLegacy))]
    [CanEditMultipleObjects]
    public sealed class MMInputUnityLegacyInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
            {
                if (GUILayout.Button("初期割り当てを追加（不足分のみ）"))
                {
                    foreach (UnityEngine.Object item in targets)
                        ((MMInputUnityLegacy)item).AddDefaultBindings();
                    serializedObject.Update();
                }
            }

            EditorGUILayout.HelpBox(
                "PS4 純正・Windows USB 接続の1台目用。未追加の行だけ補完します。既存の割り当ては維持します。" +
                "方向ボタンは十字キー・左スティック・矢印キー・W/A/S/D に対応します。",
                MessageType.Info);
            DrawDefaultInspector();

            if (EditorApplication.isPlaying)
            {
                var input = (MMInputUnityLegacy)target;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("現在の入力値", EditorStyles.boldLabel);
                string[] names = Input.GetJoystickNames();
                EditorGUILayout.LabelField("コントローラー1",
                    names.Length > 0 && !string.IsNullOrEmpty(names[0]) ? names[0] : "未接続");
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.Vector2Field("十字キー", input.DirectionPad);
                    EditorGUILayout.Vector2Field("MoveStick", input.Axis(IMMInput.AxisType.Move));
                    EditorGUILayout.Vector2Field("LookStick", input.Axis(IMMInput.AxisType.Look));
                    EditorGUILayout.FloatField("L2", input.Trigger(IMMInput.TriggerType.L2));
                    EditorGUILayout.FloatField("R2", input.Trigger(IMMInput.TriggerType.R2));
                }
                Repaint();
            }
        }
    }

    /// <summary>
    /// 旧 Input の専用軸を登録する。既存項目は変更・削除しない。
    /// 実行時アセンブリへの参照なしで動くよう、Editor 側だけで完結させる。
    /// </summary>
    [InitializeOnLoad]
    public static class MMInputUnityLegacyAxisSetup
    {
        private const string SettingsPath = "ProjectSettings/InputManager.asset";

        // axis は0始まり。joyNum は1台目を指定するため1。
        private static readonly (string name, int axis, bool invert, float dead)[] Definitions =
        {
            ("MMInput_PS4_USB_J1_MoveX", 0, false, 0.15f),
            ("MMInput_PS4_USB_J1_MoveY", 1, true,  0.15f),
            ("MMInput_PS4_USB_J1_LookX", 2, false, 0.15f),
            ("MMInput_PS4_USB_J1_LookY", 5, true,  0.15f),
            ("MMInput_PS4_USB_J1_L2",    3, false, 0f),
            ("MMInput_PS4_USB_J1_R2",    4, false, 0f),
            // 十字キーは7・8軸。上下は最初から上が正なので反転しない。
            ("MMInput_PS4_USB_J1_DirectionX", 6, false, 0.1f),
            ("MMInput_PS4_USB_J1_DirectionY", 7, false, 0.1f),
        };

        static MMInputUnityLegacyAxisSetup()
        {
            EditorApplication.delayCall += Register;
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.ExitingEditMode)
                    Register();
            };
        }

        [MenuItem("Tools/MMInput/Legacy 軸を登録")]
        public static void Register()
        {
            if (EditorApplication.isPlaying)
                return;

            UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(SettingsPath);
            if (assets.Length == 0)
                throw new InvalidOperationException("InputManager.asset が見つかりません。");

            using (var serializedObject = new SerializedObject(assets[0]))
            {
                serializedObject.Update();
                SerializedProperty axes = serializedObject.FindProperty("m_Axes");
                if (axes == null || !axes.isArray)
                    throw new InvalidOperationException("Input Manager の軸一覧を取得できません。");

                var existing = new HashSet<string>();
                for (int i = 0; i < axes.arraySize; i++)
                {
                    existing.Add(axes.GetArrayElementAtIndex(i)
                        .FindPropertyRelative("m_Name").stringValue);
                }

                bool changed = false;
                foreach (var definition in Definitions)
                {
                    if (existing.Contains(definition.name))
                        continue;

                    int index = axes.arraySize;
                    axes.arraySize++;
                    SerializedProperty axis = axes.GetArrayElementAtIndex(index);

                    // 配列追加時に直前の値が複製されても設定が混ざらないよう全項目を設定する。
                    axis.FindPropertyRelative("m_Name").stringValue = definition.name;
                    axis.FindPropertyRelative("descriptiveName").stringValue = "";
                    axis.FindPropertyRelative("descriptiveNegativeName").stringValue = "";
                    axis.FindPropertyRelative("negativeButton").stringValue = "";
                    axis.FindPropertyRelative("positiveButton").stringValue = "";
                    axis.FindPropertyRelative("altNegativeButton").stringValue = "";
                    axis.FindPropertyRelative("altPositiveButton").stringValue = "";
                    axis.FindPropertyRelative("gravity").floatValue = 0f;
                    axis.FindPropertyRelative("dead").floatValue = definition.dead;
                    axis.FindPropertyRelative("sensitivity").floatValue = 1f;
                    axis.FindPropertyRelative("snap").boolValue = false;
                    axis.FindPropertyRelative("invert").boolValue = definition.invert;
                    axis.FindPropertyRelative("type").intValue = 2;
                    axis.FindPropertyRelative("axis").intValue = definition.axis;
                    axis.FindPropertyRelative("joyNum").intValue = 1;
                    existing.Add(definition.name);
                    changed = true;
                }

                if (!changed)
                    return;

                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                AssetDatabase.SaveAssetIfDirty(assets[0]);
            }
        }
    }

    public sealed class MMInputUnityLegacyAxisBuildSetup : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            MMInputUnityLegacyAxisSetup.Register();
        }
    }
}
#endif
