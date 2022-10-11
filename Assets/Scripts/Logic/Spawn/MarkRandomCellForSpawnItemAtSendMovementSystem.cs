using Base;
using Data.Board;
using Data.Cell;
using Data.Global;
using Data.Item;
using Extensions;
using Morpeh;
using Morpeh.Globals;
using Services;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Logic.Spawn
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MarkRandomCellForSpawnItemAtSendMovementSystem : LateUpdateSystemBase
    {
        private Filter _boardFilter;
        private Filter _itemsFilter;

        private GlobalEventInt _sendMovement;

        public override void OnAwake()
        {
            _boardFilter = World.Filter
                .With<BoardComponent>();
            _itemsFilter = World.Filter
                .With<ItemComponent>()
                .With<RectTransformComponent>();

            _sendMovement = GlobalEventsProvider.Instance.SendMovement;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!_sendMovement)
            {
                return;
            }

            ref var boardComponent = ref _boardFilter.First().GetComponent<BoardComponent>();
            var itemsMatrix = MatrixExtensions.CreateItemsMatrix(_itemsFilter, boardComponent.MatrixDimensions);
            var cellsMatrix = boardComponent.BoardMatrix;

            MarkCellForSpawnItem(itemsMatrix, cellsMatrix);
        }

        private void MarkCellForSpawnItem(Entity[,] itemsMatrix, Entity[,] cellsMatrix)
        {
            var itemsMatrixCount = itemsMatrix.GetNotNullLenght();
            if (itemsMatrixCount >= cellsMatrix.Length)
            {
                return;
            }
            
            Vector2Int positionInMatrix;
            var values = itemsMatrix.GetLength(0) * itemsMatrix.GetLength(1);
            do
            {
                var index = Random.Range(0, values);
                positionInMatrix = new Vector2Int(
                    index / itemsMatrix.GetLength(0), index % itemsMatrix.GetLength(0));

            } while (itemsMatrix[positionInMatrix.x, positionInMatrix.y] != null);

            var cellEntity = cellsMatrix[positionInMatrix.x, positionInMatrix.y];
            cellEntity.SetComponent(new CreateItemOnMeMarker { Delay = 0.12f });
        }
    }
}