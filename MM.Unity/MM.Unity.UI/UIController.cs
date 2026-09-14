using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace mm.unity.ui
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Binding[] bindings;

        [SerializeField] private UnityEvent<Selectable> confirmed;
        [SerializeField] private UnityEvent<Selectable> selectionChanged;

        [SerializeField] private UnityEvent cancelEvent;

        private Dictionary<Selectable, UnityAction> confirmActionDict;

        public Action Cancelled { get; set; }
        public Action<Selectable> Confirmed { get; set; }
        public Action<Selectable> SelectionChanged { get; set; }

        public void Select(Selectable selectable)
            => UpdateSelection(selectable);

        public void Up()
        {
            var current = EventSystem.current.currentSelectedGameObject;
            if (current != null && current.TryGetComponent<Selectable>(out var selectable))
            {
                if (selectable != null)
                {
                    var next = selectable.FindSelectableOnUp();
                    UpdateSelection(next);
                }
            }
        }

        public void Down()
        {
            var current = EventSystem.current.currentSelectedGameObject;
            if (current != null && current.TryGetComponent<Selectable>(out var selectable))
            {
                if (selectable != null)
                {
                    var next = selectable.FindSelectableOnDown();
                    UpdateSelection(next);
                }
            }
        }

        public void Left()
        {
            var current = EventSystem.current.currentSelectedGameObject;
            if (current != null && current.TryGetComponent<Selectable>(out var selectable))
            {
                if (selectable != null)
                {
                    var next = selectable.FindSelectableOnLeft();
                    UpdateSelection(next);
                }
            }
        }

        public void Right()
        {
            var current = EventSystem.current.currentSelectedGameObject;
            if (current != null && current.TryGetComponent<Selectable>(out var selectable))
            {
                if (selectable != null)
                {
                    var next = selectable.FindSelectableOnRight();
                    UpdateSelection(next);
                }
            }
        }

        public void Confirm()
        {
            var current = EventSystem.current.currentSelectedGameObject;
            if (current != null && current.TryGetComponent<Selectable>(out var selectable))
            {
                if (confirmActionDict.TryGetValue(selectable, out var action))
                {
                    action?.Invoke();
                }
            }
        }

        public void Cancel()
        {
            Cancelled?.Invoke();
            cancelEvent?.Invoke();
        }

        private void Awake()
        {
            confirmActionDict = new Dictionary<Selectable, UnityAction>();
            foreach (var binding in bindings)
            {
                if (binding.Selectable != null)
                {
                    confirmActionDict[binding.Selectable] =
                        () =>
                        {
                            binding.InteractAction?.Invoke();
                            confirmed?.Invoke(binding.Selectable);
                            Confirmed?.Invoke(binding.Selectable);
                        };
                }
            }
        }

        private void OnEnable()
        {
            foreach (var binding in bindings)
            {
                if (binding.Selectable != null &&
                    confirmActionDict.TryGetValue(binding.Selectable, out var action))
                {
                    binding.Register(action);
                }
            }
        }

        private void OnDisable()
        {
            foreach (var binding in bindings)
            {
                if (binding.Selectable != null &&
                    confirmActionDict.TryGetValue(binding.Selectable, out var action))
                {
                    binding.Unregister(action);
                }
            }
        }

        private void UpdateSelection(Selectable selectable)
        {
            if (selectable != null)
            {
                var next = selectable.gameObject;
                var changed = EventSystem.current.currentSelectedGameObject != next;
                if (changed)
                {
                    EventSystem.current.SetSelectedGameObject(next);
                    selectionChanged?.Invoke(selectable);
                    SelectionChanged?.Invoke(selectable);
                }
            }
        }

        [System.Serializable]
        private class Binding
        {
            public Selectable Selectable;
            public UnityEvent InteractAction;
            public bool RegisterButton = true;

            public void Register(UnityAction action)
            {
                if (RegisterButton && Selectable is Button button)
                {
                    button.onClick.AddListener(action);
                }
            }

            public void Unregister(UnityAction action)
            {
                if (RegisterButton && Selectable is Button button)
                {
                    button.onClick.RemoveListener(action);
                }
            }
        }
    }
}
