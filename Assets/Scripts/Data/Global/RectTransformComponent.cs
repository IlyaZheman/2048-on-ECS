using System;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Data.Global
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct RectTransformComponent : IComponent
    {
        public RectTransform RectTransform;
    }
}