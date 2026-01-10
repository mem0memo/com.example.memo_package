using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace mm.view
{
    public class FocusReceiver :
        MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField]
        private UnityEvent enterEvent;
        [SerializeField]
        private UnityEvent outEvent;

        public void OnPointerEnter(PointerEventData eventData)
        {
            enterEvent.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            outEvent.Invoke();
        }
    }
}