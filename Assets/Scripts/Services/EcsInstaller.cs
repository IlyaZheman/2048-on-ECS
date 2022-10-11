using System.Collections.Generic;
using Logic.Board;
using Logic.Clear;
using Logic.Gameplay;
using Logic.Movement;
using Logic.Spawn;
using Morpeh;
using UnityEngine;

namespace Services
{
    public class EcsInstaller : MonoBehaviour
    {
        private World _world;

        private void OnEnable()
        {
            _world = World.Default;

            var systemGroup = _world.CreateSystemsGroup();
            systemGroup.AddInitializer(new CreateLevelAtStartGame());

            var systems = new List<ISystem>
            {
                // Create and configure the board on CreateLevel
                new CreateMatrixSystem(),
                new SetupBoardSettingsSystem(),
                new SetCellsToCorrectIndexInMatrixSystem(),
                new SetCellsToCorrectPositionSystem(),

                // Start game
                new MarkRandomCellForSpawnItemAtCreateLevelSystem(),
                new SpawnItemsOnMarkedCellsSystem(),
                
                // Gameplay
                new DesktopMovementSystem(),
                new ResetItemsSystem(),
                new MarkItemsToMergeOrMoveSystem(),
                new MergeItemsSystem(),
                new MoveItemsSystem(),
                
                // Late Update Systems
                new MarkRandomCellForSpawnItemAtSendMovementSystem(),
                new RemoveEntitiesSystem(),
            };

            foreach (var system in systems)
            {
                systemGroup.AddSystem(system);
            }

            _world.AddSystemsGroup(0, systemGroup);
        }
    }
}