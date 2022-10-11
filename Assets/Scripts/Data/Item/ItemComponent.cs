using System;
using Logic.Gameplay;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Data.Item
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct ItemComponent : IComponent
    {
        [SerializeField] private ScoreView scoreView;

        public Vector2Int PositionInMatrix;
        public bool HasMerged;

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                if (value is < 1 or > 11)
                {
                    throw new IndexOutOfRangeException($"{nameof(value)} has gone beyond colors");
                }
                
                _score = value;
                scoreView.SetScore(value);
            }
        }
    }
}