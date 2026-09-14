using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static mm.unity.input.IMMInput;

namespace mm.unity.input
{
    public class MMInputInputSystem : MMInputBase
    {
        [Header("Bindings")]
        [SerializeField] InputActionReference confirmAction;
        [SerializeField] InputActionReference cancelAction;

        [Header("Axis")]
        [SerializeField] InputActionReference moveAction;
        [SerializeField] InputActionReference lookAction;
        [Header("Trigger")]
        [SerializeField] InputActionReference l2Action;
        [SerializeField] InputActionReference r2Action;

        private Dictionary<KeyType, InputActionReference> actionDict;
        private Dictionary<KeyType, Action<InputAction.CallbackContext>> startActionDict;
        private Dictionary<KeyType, Action<InputAction.CallbackContext>> cancelActionDict;

        private void Awake()
        {
            actionDict = new Dictionary<KeyType, InputActionReference>()
            {
                { KeyType.Confirm, confirmAction},
                { KeyType.Cancel, cancelAction},
                { KeyType.L2, l2Action},
                { KeyType.R2, r2Action},
            };

            startActionDict = new Dictionary<KeyType, Action<InputAction.CallbackContext>>();
            cancelActionDict = new Dictionary<KeyType, Action<InputAction.CallbackContext>>();
            foreach (KeyType key in actionDict.Keys)
            {
                startActionDict[key] = ctx => SendEvent(key, IMMInput.EventType.Down);
                cancelActionDict[key] = ctx => SendEvent(key, IMMInput.EventType.Up);
            }
        }

        private void OnEnable()
        {
            foreach (KeyType key in actionDict.Keys)
            {
                if (actionDict[key] == null)
                {
                    continue;
                }

                actionDict[key].action.started += startActionDict[key];
                actionDict[key].action.canceled += cancelActionDict[key];
                actionDict[key].action.Enable();
            }

            moveAction?.action.Enable();
            lookAction?.action.Enable();
        }

        private void OnDisable()
        {
            foreach (KeyType key in actionDict.Keys)
            {
                if (actionDict[key] == null)
                {
                    continue;
                }

                actionDict[key].action.started -= startActionDict[key];
                actionDict[key].action.canceled -= cancelActionDict[key];
                actionDict[key].action.Disable();
            }

            moveAction?.action.Disable();
            lookAction?.action.Disable();
        }

        protected override float GetTrigger(IMMInput.TriggerType triggerType)
        {
            return triggerType switch
            {
                IMMInput.TriggerType.L2 => l2Action == null ? 0 : l2Action.action.ReadValue<float>(),
                IMMInput.TriggerType.R2 => r2Action == null ? 0 : r2Action.action.ReadValue<float>(),
                _ => 0f,
            };
        }

        protected override Vector2 GetAxis(IMMInput.AxisType axisType)
        {
            return axisType switch
            {
                IMMInput.AxisType.Move => moveAction == null ? Vector2.zero : moveAction.action.ReadValue<Vector2>(),
                IMMInput.AxisType.Look => lookAction == null ? Vector2.zero : lookAction.action.ReadValue<Vector2>(),
                _ => Vector2.zero,
            };
        }

        protected override bool IsPressed(IMMInput.KeyType keyType)
        {
            return actionDict.TryGetValue(keyType, out var action) &&
                action != null && action.action.IsPressed();
        }

        protected override bool WasPressedThisFrame(KeyType keyType)
        {
            return actionDict.TryGetValue(keyType, out var action) &&
                action != null && action.action.WasPressedThisFrame();
        }

        protected override bool WasReleasedThisFrame(KeyType keyType)
        {
            return actionDict.TryGetValue(keyType, out var action) &&
                action != null && action.action.WasReleasedThisFrame();
        }
    }
}