using Base;
using Data.Board;
using Data.Cell;
using Data.Global;
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
    public sealed class SetCellsToCorrectPositionSystem : UpdateSystemBase
    {
        private Filter _boardFilter;
        private Filter _cellsFilter;

        private GlobalEvent _createLevel;

        public override void OnAwake()
        {
            _boardFilter = World.Filter
                .With<BoardComponent>();
            _cellsFilter = World.Filter
                .With<CellComponent>()
                .With<RectTransformComponent>();

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

            for (var i = 0; i < _cellsFilter.Length; i++)
            {
                var cellEntity = _cellsFilter.GetEntity(i);
                ref var positionInMatrix = ref cellEntity.GetComponent<CellComponent>().PositionInMatrix;
                ref var transform = ref cellEntity.GetComponent<RectTransformComponent>().RectTransform;

                SetPosition(transform, positionInMatrix, boardComponent.BoardSettings);
                SetScale(transform, boardComponent.BoardSettings);
            }
        }

        private void SetScale(RectTransform transform, BoardSettings boardSettings)
        {
            transform.sizeDelta = new Vector2(boardSettings.CellWidth, boardSettings.CellHeight);
        }

        private void SetPosition(RectTransform transform, Vector2Int positionInMatrix, BoardSettings boardSettings)
        {
            var cellPosition = new Vector2
            {
                x = boardSettings.Indent + ((boardSettings.CellWidth + boardSettings.Indent) * positionInMatrix.x) + 
                    boardSettings.CellWidth / 2,
                y = boardSettings.Indent + ((boardSettings.CellHeight + boardSettings.Indent) * positionInMatrix.y) + 
                    boardSettings.CellHeight / 2
            };
            
            transform.anchoredPosition = cellPosition;
        }
    }
}