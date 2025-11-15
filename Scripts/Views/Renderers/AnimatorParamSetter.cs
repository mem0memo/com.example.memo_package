using mm.common;
using UnityEngine;

namespace mm.view
{
    public class AnimatorParamSetter : MonoBehaviour
    {
        [SerializeField]
        private Command<bool>[] boolCommands;
        [SerializeField]
        private Command<int>[] intCommands;
        [SerializeField]
        private Command<float>[] floatCommands;
        [SerializeField]
        private Animator animator;

        public void ExecuteBool(int index)
        {
            var command = boolCommands.GetValueOrDefault(index);
            animator.SetBool(command.parameterName, command.Value);
        }

        public void ExecuteInt(int index)
        {
            var command = intCommands.GetValueOrDefault(index);
            animator.SetInteger(command.parameterName, command.Value);
        }

        public void ExecuteFloat(int index)
        {
            var command = floatCommands.GetValueOrDefault(index);
            animator.SetFloat(command.parameterName, command.Value);
        }

        [System.Serializable]
        public struct Command<TValue>
        {
            public string parameterName;
            public TValue Value;
        }
    }
}