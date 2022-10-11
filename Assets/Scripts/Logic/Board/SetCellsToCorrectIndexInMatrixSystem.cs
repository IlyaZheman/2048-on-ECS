using Base;
using Data.Board;
using Data.Cell;
using Morpeh;
using Morpeh.Globals;
using Services;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Logic.Board
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SetCellsToCorrectIndexInMatrixSystem : UpdateSystemBase
    {
        private Filter _boardFilter;

        private GlobalEvent _createLevel;

        public override void OnAwake()
        {
            _boardFilter = World.Filter.With<BoardComponent>();

            _createLevel = GlobalEventsProvider.Instance.CreateLevel;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!_createLevel)
            {
                return;
            }

            var boardEntity = _boardFilter.First();
            ref var boardComponent = ref boardEntity.GetComponent<BoardComponent>();
            
            for (var line = 0; line < boardComponent.BoardMatrix.GetLength(0); line++)
            {
                for (var column = 0; column < boardComponent.BoardMatrix.GetLength(1); column++)
                {
                    ref var cell = ref boardComponent.BoardMatrix[line, column].GetComponent<CellComponent>();
                    cell.PositionInMatrix = new Vector2Int(line, column);
                }
            }
        }
    }
}