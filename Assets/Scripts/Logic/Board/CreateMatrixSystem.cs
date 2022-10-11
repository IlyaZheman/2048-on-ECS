using Base;
using Data.Board;
using Data.Cell;
using Data.Global;
using Morpeh;
using Morpeh.Globals;
using Services;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Logic.Board
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CreateMatrixSystem : UpdateSystemBase
    {
        private Filter _boardFilter;
        private Filter _cellsContainerFilter;

        private GlobalEvent _createLevel;

        public override void OnAwake()
        {
            _boardFilter = World.Filter
                .With<BoardComponent>();
            _cellsContainerFilter = World.Filter
                .With<CellsContainerComponent>()
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

            var cellContainerEntity = _cellsContainerFilter.First();
            ref var cellsContainerTransform =
                ref cellContainerEntity.GetComponent<RectTransformComponent>().RectTransform;
            
            boardComponent.BoardMatrix =
                new Entity[boardComponent.MatrixDimensions.x, boardComponent.MatrixDimensions.y];

            for (var line = 0; line < boardComponent.MatrixDimensions.x; line++)
            {
                for (var column = 0; column < boardComponent.MatrixDimensions.y; column++)
                {
                    var cell = Object.Instantiate(GameObjectsProvider.Instance.Cell, Vector3.zero, 
                        quaternion.identity, cellsContainerTransform);
                    var entity = cell.GetComponent<CellProvider>().Entity;

                    boardComponent.BoardMatrix[line, column] = entity;
                }
            }
        }
    }
}