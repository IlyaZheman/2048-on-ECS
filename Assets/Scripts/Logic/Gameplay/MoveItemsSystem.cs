using Base;
using Data.Cell;
using Data.Global;
using Data.Item;
using DG.Tweening;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Logic.Gameplay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MoveItemsSystem : UpdateSystemBase
    {
        private Filter _itemsFilter;

        public override void OnAwake()
        {
            _itemsFilter = World.Filter
                .With<ItemComponent>()
                .With<MoveItemMarkerComponent>()
                .With<RectTransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < _itemsFilter.Length; i++)
            {
                var entity = _itemsFilter.GetEntity(i);
                ref var itemTransform = ref entity.GetComponent<RectTransformComponent>().RectTransform;

                var targetEntity = entity.GetComponent<MoveItemMarkerComponent>().TargetCellEntity;
                ref var targetTransform = ref targetEntity.GetComponent<RectTransformComponent>().RectTransform;

                itemTransform.DOAnchorPos(targetTransform.anchoredPosition, 0.1f);

                entity.RemoveComponent<MoveItemMarkerComponent>();
            }
        }
    }
}