using System;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Data.Board
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct BoardComponent : IComponent
    {
        [HideInInspector] 
        public Entity[,] BoardMatrix;
        [Range(0f, 100f)]
        public float PercentageIndentation;
        public Vector2Int MatrixDimensions;
        public Vector2 MaxDimensions;
        public BoardSettings BoardSettings;
    }
}