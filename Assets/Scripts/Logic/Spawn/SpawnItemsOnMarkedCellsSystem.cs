using Base;
using Data.Board;
using Data.Cell;
using Data.Global;
using Data.Item;
using Morpeh;
using Services;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Logic.Spawn
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SpawnItemsOnMarkedCellsSystem : UpdateSystemBase
    {
        private Filter _cellsFilter;
        private Filter _itemsContainerFilter;
        private Filter _boardFilter;

        public override void OnAwake()
        {
            _cellsFilter = World.Filter
                .With<CellComponent>()
                .With<CreateItemOnMeMarker>()
                .With<RectTransformComponent>();
            _itemsContainerFilter = World.Filter
                .With<ItemsContainerComponent>()
                .With<RectTransformComponent>();
            _boardFilter = World.Filter
                .With<BoardComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_cellsFilter.Length == 0)
            {
                return;
            }

            ref var boardComponent = ref _boardFilter.First().GetComponent<BoardComponent>();
            ref var itemsContainerTransform =
                ref _itemsContainerFilter.First().GetComponent<RectTransformComponent>().RectTransform;
            
            for (var i = 0; i < _cellsFilter.Length; i++)
            {
                var entity = _cellsFilter.GetEntity(i);
                ref var delay = ref entity.GetComponent<CreateItemOnMeMarker>().Delay;
                
                if (delay > 0)
                {
                    delay -= deltaTime;
                    return;
                }
                
                SpawnItem(entity, itemsContainerTransform,
                    boardComponent.BoardSettings.CellWidth, boardComponent.BoardSettings.CellHeight);

                entity.RemoveComponent<CreateItemOnMeMarker>();

            }
        }

        private void SpawnItem(Entity entity, RectTransform transform, float itemWidth, float itemHeight)
        {
            ref var cellTransform = ref entity.GetComponent<RectTransformComponent>().RectTransform;

            var item = Object.Instantiate(GameObjectsProvider.Instance.Item, Vector2.zero, 
                Quaternion.identity, transform).transform as RectTransform;

            if (item == null)
            {
                throw new UnityException($"{nameof(item)}.RectTransform = null");
            }
            
            item.sizeDelta = new Vector2(itemWidth, itemHeight);
            item.anchoredPosition = cellTransform.anchoredPosition;

            ref var itemComponent = ref item.GetComponent<EntityProvider>().Entity.GetComponent<ItemComponent>();
            
            itemComponent.PositionInMatrix = entity.GetComponent<CellComponent>().PositionInMatrix;
            RandomizeItemScore(ref itemComponent);
        }

        private void RandomizeItemScore(ref ItemComponent itemComponent)
        {
            var weight = Random.Range(0, 100);
            itemComponent.Score = weight < 90 ? 1 : 2;
        }
    }
}