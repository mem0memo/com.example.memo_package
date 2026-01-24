using UnityEngine;

namespace mm
{
    public abstract class SingletonBase<T> : MonoBehaviour
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance.Equals(this))
            {
                Destroy(gameObject);
                return;
            }

            if (this is T newInstance)
            {
                Instance = newInstance;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
