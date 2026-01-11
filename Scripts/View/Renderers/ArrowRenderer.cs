using UnityEngine;

namespace mm.view
{
    public class ArrowRenderer : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer lineRenderer;
        [SerializeField]
        private Transform middle;
        [SerializeField]
        private Transform target;

        public Transform Middle => middle;

        public Transform Target => target;

        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            DrawQuadraticBezierCurve(
                transform.position,
                middle.position,
                target.position);
        }

        void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            lineRenderer.positionCount = 200;
            float t = 0f;
            Vector3 B = new Vector3(0, 0, 0);
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
                lineRenderer.SetPosition(i, B);
                t += (1 / (float)lineRenderer.positionCount);
            }
        }
    }
}