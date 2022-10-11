using System;
using Base;
using Data.Cell;
using Data.Global;
using Morpeh;
using Morpeh.Globals;
using Services;
using Unity.IL2CPP.CompilerServices;
using Random = UnityEngine.Random;

namespace Logic.Spawn
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MarkRandomCellForSpawnItemAtCreateLevelSystem : UpdateSystemBase
    {
        private Filter _cellsFilter;

        private GlobalEvent _createLevel;

        public override void OnAwake()
        {
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

            var cellIndex = MarkRandomCell();
            MarkRandomCell(cellIndex);
        }

        private int MarkRandomCell(int value = -1)
        {
            var randomCell = value == -1
                ? SelectRandomCell(_cellsFilter.Length)
                : SelectRandomCell(_cellsFilter.Length, value);

            var entity = _cellsFilter.GetEntity(randomCell);
            entity.SetComponent(new CreateItemOnMeMarker { Delay = 0f });

            return randomCell;
        }

        private int SelectRandomCell(int values, int value = -1)
        {
            int randomCell, safeValue = 0;

            do
            {
                randomCell = Random.Range(0, _cellsFilter.Length);

                if (safeValue++ > values * 2)
                {
                    throw new ArgumentNullException();
                }
                
            } while (randomCell == value);

            return randomCell;
        }
    }
}