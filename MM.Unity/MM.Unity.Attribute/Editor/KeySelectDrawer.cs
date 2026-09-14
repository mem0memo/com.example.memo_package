using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace mm.unity.attribute.editor
{
    [CustomPropertyDrawer(typeof(KeySelectAttribute))]
    public sealed class KeySelectDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type fieldType = fieldInfo?.FieldType;
            bool isKeyField = fieldType == typeof(KeyCode) ||
                fieldType == typeof(KeyCode[]) || fieldType == typeof(List<KeyCode>);
            if (property.propertyType != SerializedPropertyType.Enum || !isKeyField)
            {
                EditorGUI.LabelField(position, label.text, "KeyCode に指定してください");
                return;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            bool previousMixed = EditorGUI.showMixedValue;
            try
            {
                EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
                Rect fieldRect = EditorGUI.PrefixLabel(position, label);
                string text = property.hasMultipleDifferentValues
                    ? "—"
                    : KeyDropdown.DisplayName((KeyCode)property.intValue);

                if (EditorGUI.DropdownButton(fieldRect, new GUIContent(text), FocusType.Keyboard))
                {
                    // ポップアップを開いたまま配列が変わる可能性があるため、
                    // SerializedProperty 自体は選択コールバックに保持しない。
                    UnityEngine.Object[] targets = property.serializedObject.targetObjects;
                    string path = property.propertyPath;
                    var dropdown = new KeyDropdown(key => Apply(targets, path, key));
                    dropdown.Show(fieldRect);
                }
            }
            finally
            {
                EditorGUI.showMixedValue = previousMixed;
                EditorGUI.EndProperty();
            }
        }

        // 標準 Inspector と既存の OnGUI ベースの Inspector の両方に対応する。
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedObject serializedObject = property.serializedObject;
            string path = property.propertyPath;
            return new IMGUIContainer(() =>
            {
                if (serializedObject.targetObject == null)
                    return;

                serializedObject.UpdateIfRequiredOrScript();
                SerializedProperty current = serializedObject.FindProperty(path);
                if (current == null)
                    return;

                Rect rect = EditorGUILayout.GetControlRect();
                OnGUI(rect, current, new GUIContent(current.displayName, current.tooltip));
            });
        }

        private static void Apply(UnityEngine.Object[] targets, string path, KeyCode key)
        {
            var alive = new List<UnityEngine.Object>();
            foreach (UnityEngine.Object target in targets)
            {
                if (target != null)
                    alive.Add(target);
            }

            if (alive.Count == 0)
                return;

            using (var serializedObject = new SerializedObject(alive.ToArray()))
            {
                serializedObject.Update();
                SerializedProperty property = serializedObject.FindProperty(path);
                if (property == null || property.propertyType != SerializedPropertyType.Enum)
                    return;

                // enumValueIndex ではなく、KeyCode の実際の数値を保存する。
                // ApplyModifiedProperties により Undo と Prefab の変更記録に対応する。
                property.intValue = (int)key;
                serializedObject.ApplyModifiedProperties();
            }
        }

        private sealed class KeyItem : AdvancedDropdownItem
        {
            public readonly KeyCode Key;

            public KeyItem(KeyCode key, string name) : base(name)
            {
                Key = key;
            }
        }

        private sealed class KeyDropdown : AdvancedDropdown
        {
            private readonly Action<KeyCode> selected;

            // PS4 純正コントローラーを Windows に直接 USB 接続した場合の参考表。
            // 自動検出ではない。変換ソフト・別の機種・OS では一致しない場合がある。
            private static readonly string[] PlayStation4Buttons =
            {
            "□", "×", "○", "△",
            "L1", "R1", "L2 押し込み", "R2 押し込み",
            "SHARE", "OPTIONS",
            "L3 左スティック押し込み", "R3 右スティック押し込み",
            "PS ボタン", "タッチパッド押し込み"
        };

            public KeyDropdown(Action<KeyCode> selected) : base(new AdvancedDropdownState())
            {
                this.selected = selected;
                minimumSize = new Vector2(460f, 380f);
            }

            protected override AdvancedDropdownItem BuildRoot()
            {
                var root = new AdvancedDropdownItem("キーを検索・選択");
                root.AddChild(new KeyItem(KeyCode.None, DisplayName(KeyCode.None)));

                var keyboard = new AdvancedDropdownItem("キーボード");
                var mouse = new AdvancedDropdownItem("マウス");
                var joystick = new AdvancedDropdownItem("コントローラー（全台 / PS4参考）");
                root.AddChild(keyboard);
                root.AddChild(mouse);
                root.AddChild(joystick);

                var devices = new Dictionary<string, AdvancedDropdownItem>();
                var seen = new HashSet<int>();
                seen.Add((int)KeyCode.None);

                foreach (string name in Enum.GetNames(typeof(KeyCode)))
                {
                    var key = (KeyCode)Enum.Parse(typeof(KeyCode), name);
                    // 同じ数値を持つ別名は重複表示しない。
                    if (!seen.Add((int)key))
                        continue;

                    AdvancedDropdownItem parent = keyboard;
                    if (name.StartsWith("Mouse", StringComparison.Ordinal))
                    {
                        parent = mouse;
                    }
                    else if (name.StartsWith("JoystickButton", StringComparison.Ordinal))
                    {
                        parent = joystick;
                    }
                    else if (name.StartsWith("Joystick", StringComparison.Ordinal))
                    {
                        int end = name.IndexOf("Button", StringComparison.Ordinal);
                        if (end > 0)
                        {
                            string device = name.Substring(0, end);
                            if (!devices.TryGetValue(device, out parent))
                            {
                                parent = new AdvancedDropdownItem(device);
                                devices.Add(device, parent);
                                root.AddChild(parent);
                            }
                        }
                    }

                    parent.AddChild(new KeyItem(key, DisplayName(key)));
                }

                return root;
            }

            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                if (item is KeyItem keyItem)
                    selected(keyItem.Key);
            }

            public static string DisplayName(KeyCode key)
            {
                string name = key.ToString();
                if (name.StartsWith("Joystick", StringComparison.Ordinal))
                {
                    int marker = name.IndexOf("Button", StringComparison.Ordinal);
                    if (marker >= 0 && int.TryParse(name.Substring(marker + 6), out int number))
                    {
                        string description = number >= 0 && number < PlayStation4Buttons.Length
                            ? PlayStation4Buttons[number]
                            : "対応未定義";
                        return $"{name}（PS4参考：{description}）";
                    }
                }

                switch (key)
                {
                    case KeyCode.None: return "None（未設定）";
                    case KeyCode.Mouse0: return "Mouse0（マウス左）";
                    case KeyCode.Mouse1: return "Mouse1（マウス右）";
                    case KeyCode.Mouse2: return "Mouse2（マウス中央）";
                    case KeyCode.Return: return "Return（Enter）";
                    case KeyCode.Space: return "Space（スペース）";
                    case KeyCode.UpArrow: return "UpArrow（上矢印）";
                    case KeyCode.DownArrow: return "DownArrow（下矢印）";
                    case KeyCode.LeftArrow: return "LeftArrow（左矢印）";
                    case KeyCode.RightArrow: return "RightArrow（右矢印）";
                    default: return key.ToString();
                }
            }
        }
    }
}