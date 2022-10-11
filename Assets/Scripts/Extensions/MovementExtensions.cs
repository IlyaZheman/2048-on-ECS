using System;
using Data.Movement;
using UnityEngine;

namespace Extensions
{
    public static class MovementExtensions
    {
        public static Vector2Int GetConvertMovement(this Movements movement)
        {
            return movement switch
            {
                Movements.Up => Vector2Int.up,
                Movements.Right => Vector2Int.right,
                Movements.Down => Vector2Int.down,
                Movements.Left => Vector2Int.left,
                _ => throw new ArgumentOutOfRangeException(nameof(movement), movement, null)
            };
        }

        public static bool IsHorizontalDirection(this Vector2Int direction)
        {
            if (direction == Vector2Int.left || direction == Vector2Int.right)
            {
                return true;
            }

            if (direction == Vector2Int.up || direction == Vector2Int.down)
            {
                return false;
            }

            throw new ArgumentOutOfRangeException();
        }

        public static bool IsPositiveDirection(this Vector2Int direction)
        {
            if (direction.x > 0 || direction.y > 0)
            {
                return true;
            }

            if (direction.x < 0 || direction.y < 0)
            {
                return false;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}