using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace mm.view
{
    public class ClickReceiver :
        MonoBehaviour,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField]
        private UnityEvent clickEvent;
        [SerializeField]
        private UnityEvent downEvent;
        [SerializeField]
        private UnityEvent upEvent;

        public Action Clicked { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickEvent?.Invoke();
            Clicked?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            downEvent?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            upEvent?.Invoke();
        }
    }
}