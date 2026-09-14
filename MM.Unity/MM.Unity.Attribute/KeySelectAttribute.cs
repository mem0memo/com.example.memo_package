using System;
using UnityEngine;

namespace mm.unity.attribute
{
    /// <summary>
    /// KeyCode を検索・分類付きの選択欄で表示する。
    /// KeyCode 配列・リストでは各要素に適用する。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class KeySelectAttribute : PropertyAttribute
    {
    }
}

