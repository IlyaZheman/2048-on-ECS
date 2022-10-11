using Base;
using Data.Movement;
using Morpeh.Globals;
using Services;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Logic.Movement
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DesktopMovementSystem : UpdateSystemBase
    {
        private GlobalEvent _createLevel;
        
        public override void OnAwake()
        {
            _createLevel = GlobalEventsProvider.Instance.CreateLevel;
        }

        public override void OnUpdate(float deltaTime)
        {
            // if (!_createLevel.IsPublished)
            // {
            //     return;
            // }

            SendMovement();

            // TODO: Удалить после дебага
            // var a = GlobalEventsProvider.Instance.SendMovement;
            // if (a)
            // {
            //     var b = a.BatchedChanges.Peek();
            //     Debug.Log(((Movements)b).ToString());
            // }
        }

        private void SendMovement()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GlobalEventsProvider.Instance.SendMovement.NextFrame((int) Movements.Up);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GlobalEventsProvider.Instance.SendMovement.NextFrame((int) Movements.Right);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                GlobalEventsProvider.Instance.SendMovement.NextFrame((int) Movements.Down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                GlobalEventsProvider.Instance.SendMovement.NextFrame((int) Movements.Left);
            }
        }
    }
}