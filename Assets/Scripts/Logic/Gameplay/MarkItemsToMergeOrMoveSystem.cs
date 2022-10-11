using Base;
using Data.Board;
using Data.Cell;
using Data.Global;
using Data.Item;
using Data.Movement;
using Extensions;
using Morpeh;
using Morpeh.Globals;
using Services;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Logic.Gameplay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MarkItemsToMergeOrMoveSystem : UpdateSystemBase
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
            var cellsMatrix = boardComponent.BoardMatrix;
            var itemsMatrix = MatrixExtensions.CreateItemsMatrix(_itemsFilter, boardComponent.MatrixDimensions);
            var movement = ((Movements)_sendMovement.BatchedChanges.Peek()).GetConvertMovement();

            MarkingForMove(itemsMatrix, cellsMatrix, boardComponent.MatrixDimensions, movement);
        }

        private void MarkingForMove(Entity[,] itemsMatrix, Entity[,] cellsMatrix,
            Vector2Int matrixDimensions, Vector2Int direction)
        {
            var dimensionX = direction.IsHorizontalDirection() ? 1 : 0;
            var dimensionY = direction.IsHorizontalDirection() ? 0 : 1;

            var start = direction.IsPositiveDirection() ? cellsMatrix.GetLength(dimensionY) - 1 : 0;
            var increment = direction.IsPositiveDirection() ? -1 : 1;

            for (var i = 0; i < cellsMatrix.GetLength(dimensionX); i++)
            {
                for (var j = start; j >= 0 && j < cellsMatrix.GetLength(dimensionY); j += increment)
                {
                    var itemEntity = direction.IsHorizontalDirection() ? itemsMatrix[j, i] : itemsMatrix[i, j];
                    itemsMatrix = MarkingItem(itemEntity, itemsMatrix, cellsMatrix, matrixDimensions, direction);
                }
            }
        }

        private Entity[,] MarkingItem(Entity itemEntity, Entity[,] itemsMatrix, Entity[,] cellsMatrix,
            Vector2Int matrixDimensions, Vector2Int direction)
        {
            if (itemEntity == null)
            {
                return itemsMatrix;
            }

            ref var itemComponent = ref itemEntity.GetComponent<ItemComponent>();

            // Merge
            var entityToMerge = FindCellToMerge(itemsMatrix, cellsMatrix, direction, itemComponent.PositionInMatrix);
            if (entityToMerge != null && itemComponent.HasMerged)
            {
                ref var targetItemComponent = ref entityToMerge.GetComponent<ItemComponent>();
                if (itemComponent.Score == targetItemComponent.Score && targetItemComponent.HasMerged)
                {
                    itemEntity.SetComponent(new MergeItemsMarkerComponent { TargetItemEntity = entityToMerge });

                    itemComponent.PositionInMatrix = entityToMerge.GetComponent<ItemComponent>().PositionInMatrix;
                    itemComponent.HasMerged = false;
                    targetItemComponent.HasMerged = false;

                    return MatrixExtensions.CreateItemsMatrix(_itemsFilter, matrixDimensions);
                }
            }

            // Move
            var emptyCell = FindEmptyCell(itemsMatrix, cellsMatrix, direction, itemComponent.PositionInMatrix);
            if (emptyCell != null)
            {
                itemEntity.SetComponent(new MoveItemMarkerComponent { TargetCellEntity = emptyCell });

                itemComponent.PositionInMatrix = emptyCell.GetComponent<CellComponent>().PositionInMatrix;

                return MatrixExtensions.CreateItemsMatrix(_itemsFilter, matrixDimensions);
            }

            return itemsMatrix;
        }

        private static Entity FindCellToMerge(Entity[,] itemsMatrix, Entity[,] cellsMatrix,
            Vector2Int direction, Vector2Int positionInMatrix)
        {
            var position = direction.IsHorizontalDirection() ? positionInMatrix.x : positionInMatrix.y;
            var start = direction.IsPositiveDirection() ? position + 1 : position - 1;
            var dimensionY = direction.IsHorizontalDirection() ? 0 : 1;
            var increment = direction.IsPositiveDirection() ? 1 : -1;

            for (var i = start; i >= 0 && i < cellsMatrix.GetLength(dimensionY); i += increment)
            {
                var item = direction.IsHorizontalDirection()
                    ? itemsMatrix[i, positionInMatrix.y]
                    : itemsMatrix[positionInMatrix.x, i];

                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }

        private static Entity FindEmptyCell(Entity[,] itemsMatrix, Entity[,] cellsMatrix,
            Vector2Int direction, Vector2Int positionInMatrix)
        {
            Entity lastEmptyCell = null;

            var position = direction.IsHorizontalDirection() ? positionInMatrix.x : positionInMatrix.y;
            var start = direction.IsPositiveDirection() ? position + 1 : position - 1;
            var dimensionY = direction.IsHorizontalDirection() ? 0 : 1;
            var increment = direction.IsPositiveDirection() ? 1 : -1;

            for (var i = start; i >= 0 && i < cellsMatrix.GetLength(dimensionY); i += increment)
            {
                var pos = direction.IsHorizontalDirection()
                    ? new Vector2Int(i, positionInMatrix.y)
                    : new Vector2Int(positionInMatrix.x, i);

                if (itemsMatrix[pos.x, pos.y] == null)
                {
                    lastEmptyCell = cellsMatrix[pos.x, pos.y];
                    continue;
                }

                break;
            }

            return lastEmptyCell;
        }
    }
}