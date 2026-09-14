using UnityEngine;

namespace mm.unity
{
    public class DebugLog : MonoBehaviour
    {
        [SerializeField]
        private string message = "log";

        public void Log()
        {
            Debug.Log(message);
        }

        private bool inputChecked;

        private void Update()
        {
            if (inputChecked)
                return;

            inputChecked = true;

            Debug.Log(
                $"Update動作中 / " +
                $"Mouseあり={UnityEngine.InputSystem.Mouse.current != null} / " +
                $"Focus={Application.isFocused} / " +
                $"UpdateMode={UnityEngine.InputSystem.InputSystem.settings.updateMode} / " +
                $"TimeScale={Time.timeScale}");
        }
    }
}
