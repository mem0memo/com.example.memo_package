using Unity.Mathematics;
using UnityEngine;

namespace mm.view
{
    public class TrackingUI : MonoBehaviour
    {
        public Transform Target;
        public RectTransform UITransform;
        public float2 Offset = new float2(0, -0.2f);

        private void Update()
        {
            if (Target == null || UITransform == null || Camera.main == null)
            {
                return;
            }

            Vector3 targetWorldPosition = (float3)Target.position + math.float3(Offset, 0);
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetWorldPosition);
            if (screenPosition.z < 0)
            {
                // HPバーを非表示にする
                UITransform.gameObject.SetActive(false);
                return;
            }

            UITransform.gameObject.SetActive(true);
            UITransform.position = screenPosition;
        }
    }
}
