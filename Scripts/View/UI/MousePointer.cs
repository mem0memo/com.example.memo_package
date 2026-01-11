using System;
using UnityEngine;
using UnityEngine.Events;

namespace mm.view
{
    public class MousePointer : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<GameObject> clickEvent;
        public Action<GameObject> OnClick { get; set; }

        // UnityのUpdate()でクリックを検出
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // 何かに当たった
                    OnClick?.Invoke(hit.collider.gameObject);
                    clickEvent?.Invoke(hit.collider.gameObject);
                }
                else
                {
                    OnClick?.Invoke(default);
                    clickEvent?.Invoke(default);
                }
            }
        }
    }
}