using System;
using UnityEngine;
using UnityEngine.Events;

namespace mm.unity.input
{

    public sealed class MMInputEvents : MonoBehaviour
    {
        [Serializable]
        private sealed class Binding
        {
            public IMMInput.KeyType key;
            public IMMInput.EventType eventType;
            public UnityEvent onInput = new();
        }

        [SerializeField] private MMInputBase source;
        [SerializeField] private Binding[] bindings = Array.Empty<Binding>();

        private void OnEnable()
        {
            if (source != null)
            {
                source.FrameEvent += KeyEvent;
            }
        }

        private void OnDisable()
        {
            if (source != null)
            {
                source.FrameEvent -= KeyEvent;
            }
        }

        private void KeyEvent(IMMInput.FrameInfo info)
        {
            foreach (var binding in bindings)
            {
                if (binding.key == info.Key &&
                    binding.eventType == info.Event)
                {
                    binding.onInput.Invoke();
                }
            }
        }
    }
}
