using System;
using System.Collections;
using UnityEngine;

namespace mm.unity
{
    public static class CoroutineExtension
    {
        public static IEnumerator OnCompleted(this IEnumerator coroutine, Action onComplete)
        {
            yield return coroutine;
            onComplete?.Invoke();
        }

        public static IEnumerator Chain(this IEnumerator coroutine, IEnumerator nextCoroutine)
        {
            yield return coroutine;
            yield return nextCoroutine;
        }

        public static IEnumerator Chain(this IEnumerator coroutine, double waitSeconds)
        {
            yield return coroutine;
            yield return new WaitForSeconds((float)waitSeconds);
        }

        public static void Run(this IEnumerator coroutine, MonoBehaviour monoBehaviour)
        {
            monoBehaviour.StartCoroutine(coroutine);
        }
    }
}
