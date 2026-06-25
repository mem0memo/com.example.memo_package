using System.Collections.Generic;
using System.Linq;
using mm.core.flow;
using UnityEngine;

namespace mm.unity.flow
{
    public class InfraProvider : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> infraObjectList = new List<GameObject>();

        private List<IInfrastructure> infraComponentList = new List<IInfrastructure>();
        private List<IInfrastructure> registerInfraList = new List<IInfrastructure>();

        public void Register(IInfrastructure infra)
        {
            if (registerInfraList.Contains(infra))
            {
                return;
            }

            registerInfraList.Add(infra);
        }

        public void Unregister(IInfrastructure infra)
        {
            if (registerInfraList.Contains(infra))
            {
                registerInfraList.Remove(infra);
            }
        }

        public T Find<T>()
        where T : IInfrastructure
        => FindInfra<T>();

        public bool TryFind<T>(out T result)
        where T : IInfrastructure
        {
            result = FindInfra<T>();
            return result != null;
        }

        private void Awake()
        {
            var infraObjects = infraObjectList.Distinct();
            foreach (var infraObj in infraObjects)
            {
                var infraComponents = infraObj.GetComponents<IInfrastructure>();
                infraComponentList.AddRange(infraComponents);
            }
        }

        private T FindInfra<T>()
        {
            foreach (var infra in infraComponentList)
            {
                if (infra is T value)
                {
                    return value;
                }
            }

            foreach (var infra in registerInfraList)
            {
                if (infra is T value)
                {
                    return value;
                }
            }

            Debug.LogError($"{typeof(T).FullName} is not found");
            return default;
        }
    }
}
