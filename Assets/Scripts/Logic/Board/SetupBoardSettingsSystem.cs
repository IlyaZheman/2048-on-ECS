using Base;
using Data.Board;
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
    public sealed class SetupBoardSettingsSystem : UpdateSystemBase
    {
        private Filter _boardFilter;

        private GlobalEvent _createLevel;

        private float _maxWidth;
        private float _maxHeight;
        
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
            ref var boardTransform = ref boardEntity.GetComponent<RectTransformComponent>().RectTransform;

            _maxWidth = boardComponent.MaxDimensions.x;
            _maxHeight = boardComponent.MaxDimensions.y;
            CalculateBoardRect(ref boardComponent);

            boardTransform.sizeDelta =
                new Vector2(boardComponent.BoardSettings.Width, boardComponent.BoardSettings.Height);
        }

        // TODO: Поправить 5*6
        private void CalculateBoardRect(ref BoardComponent boardComponent)
        {
            var maxWidth = _maxWidth;
            var maxHeight = _maxHeight;
            
            var matrixDimensions = boardComponent.MatrixDimensions;
            var percentageIndentation = boardComponent.PercentageIndentation;

            var orientation = matrixDimensions.x >= matrixDimensions.y ? "Horizontal" : "Vertical";

            float width, height, indent, cellWidth, cellHeight;

            if (orientation == "Horizontal")
            {
                width = maxWidth;
                indent = (width / 100) * percentageIndentation / (matrixDimensions.x + 1);
                cellWidth = (width - (indent * (matrixDimensions.x + 1))) / matrixDimensions.x;
                
                cellHeight = cellWidth;
                height = indent + ((cellHeight + indent) * matrixDimensions.y);

                if (height > _maxHeight)
                {
                    _maxWidth -= 10f;
                    CalculateBoardRect(ref boardComponent);
                }
            }
            else
            {
                height = maxHeight;
                indent = (height / 100) * percentageIndentation / (matrixDimensions.y + 1);
                cellHeight = (height - (indent * (matrixDimensions.y + 1))) / matrixDimensions.y;

                cellWidth = cellHeight;
                width = indent + ((cellWidth + indent) * matrixDimensions.x);

                if (width > _maxWidth)
                {
                    _maxHeight -= 10f;
                    CalculateBoardRect(ref boardComponent);
                }
            }
            
            ref var boardSettings = ref boardComponent.BoardSettings;
            boardSettings.Width = width;
            boardSettings.Height = height;
            boardSettings.Indent = indent;
            boardSettings.CellWidth = cellWidth;
            boardSettings.CellHeight = cellHeight;
        }
    }
}