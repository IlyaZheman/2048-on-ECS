using Data.Item;
using Morpeh;
using UnityEngine;

namespace Extensions
{
    public static class MatrixExtensions
    {
        public static Entity[,] CreateItemsMatrix(Filter itemsFilter, Vector2Int matrixDimensions)
        {
            var matrix = new Entity[matrixDimensions.x, matrixDimensions.y];

            for (var i = 0; i < itemsFilter.Length; i++)
            {
                var entity = itemsFilter.GetEntity(i);
                ref var itemComponent = ref entity.GetComponent<ItemComponent>();
                matrix[itemComponent.PositionInMatrix.x, itemComponent.PositionInMatrix.y] = entity;
            }

            return matrix;
        }

        public static int GetNotNullLenght(this Entity[,] matrix)
        {
            var count = 0;
            
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i,j] != null)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}