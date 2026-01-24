using UnityEngine;

namespace mm.flow
{
    public class TestLogger : MonoBehaviour, IMessageService
    {
        public void Send(string message, Color color)
        {
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>");
        }
    }
}
