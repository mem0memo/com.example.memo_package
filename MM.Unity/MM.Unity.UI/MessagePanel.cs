using mm.core.flow;
using UnityEngine;

namespace mm.unity
{
    public class MessagePanel : MonoBehaviour, IInfrastructure
    {
        [SerializeField]
        private TMPro.TMP_Text text;

        public void Show(string message)
        {
            text.SetText(message);
        }
    }
}
