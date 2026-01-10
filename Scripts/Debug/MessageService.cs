using TMPro;
using UnityEngine;

namespace mm.debug
{
    public class MessageService : ServiceComponentBase
    {
        [SerializeField]
        private TMP_Text text;

        public void Show(string message) => Show(message, Color.white);

        public void Show(string message, Color color)
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
