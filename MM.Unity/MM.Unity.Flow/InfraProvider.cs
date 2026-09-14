using System.Collections.Generic;
using System.Linq;
using mm.core.flow;
using UnityEngine;

namespace mm.unity.flow
{
    public class InfraProvider : MonoBehaviour, IDIResolver
    {
        [SerializeField]
        private List<GameObject> infraObjectList = new List<GameObject>();

        private List<object> infraList = new List<object>();

        T IDIResolver.Resolve<T>()
            where T : class
        {
            foreach (var infra in infraList)
            {
                if (infra is T value)
                {
                    return value;
                }
            }

            Debug.LogError($"[InfraProvider] {typeof(T).FullName} is not found");
            return default;
        }



        public void Install(object infra)
        {
            if (infra != null && infraList.Contains(infra))
            {
                return;
            }

            infraList.Add(infra);
        }


        private void Awake()
        {
            foreach (var infraObj in infraObjectList)
            {
                var components = infraObj.GetComponents<Component>()
                    .Where(c => !c.GetType().Equals(typeof(Transform)))
                    .Where(c => !c.GetType().Equals(typeof(RectTransform)));
                infraList.AddRange(components);
            }

            infraList = infraList.Distinct().ToList();
        }
    }
}
