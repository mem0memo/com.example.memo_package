using TMPro;
using UnityEngine;

namespace mm.debug
{
    public class MessageDisplay : MonoBehaviour, IMessageService
    {
        [SerializeField]
        private TMP_Text text;

        public void Send(string message, Color color)
        {
            if (text != null)
            {
                text.enabled = true;
                text.text = message;
                text.color = color;
            }
        }

        public void Hide()
        {
            if (text != null)
            {
                text.enabled = false;
            }
        }

        private void Awake()
        {
            Hide();
        }
    }
}
