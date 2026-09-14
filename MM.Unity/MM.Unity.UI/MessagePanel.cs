using System;
using System.Collections;
using UnityEngine;

namespace mm.unity
{
    public class MessagePanel : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_Text text;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private string openStateName = "Open";
        [SerializeField]
        private float openStateDuration = 0.5f;
        [SerializeField]
        private string closeStateName = "Close";
        [SerializeField]
        private float closeStateDuration = 0.5f;
        [SerializeField]
        private float showDuration = 1.0f;

        public void Show(string message, Action onComplete = null)
        {
            animator.Update(0f);
            OpenCoroutine(message, openStateDuration)
                .OnCompleted(onComplete)
                .Run(this);
        }

        public void ShowTemporary(string message, Action onComplete = null)
        {
            animator.Update(0f);
            OpenCoroutine(message, openStateDuration)
                .Chain(showDuration)
                .Chain(CloseCoroutine(closeStateDuration))
                .OnCompleted(onComplete)
                .Run(this);
        }

        public void Hide(Action onComplete = null)
        {
            animator.Update(0f);
            CloseCoroutine(closeStateDuration)
                .OnCompleted(onComplete)
                .Run(this);
        }

        private IEnumerator OpenCoroutine(string message, float openStateDuration)
        {
            text.SetText(message);
            animator.Play(openStateName);
            yield return new WaitForSeconds(openStateDuration);
        }

        private IEnumerator CloseCoroutine(float closeStateDuration)
        {
            animator.Play(closeStateName);
            yield return new WaitForSeconds(closeStateDuration);
        }

        [ContextMenu("Show Temporary")]
        private void Show()
        {
            ShowTemporary("Hello, World!");
        }
    }
}
