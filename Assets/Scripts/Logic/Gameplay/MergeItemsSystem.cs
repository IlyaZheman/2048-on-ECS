using Base;
using Data.Clear;
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
    public sealed class MergeItemsSystem : UpdateSystemBase
    {
        private Filter _itemsFilter;

        public override void OnAwake()
        {
            _itemsFilter = World.Filter
                .With<ItemComponent>()
                .With<MergeItemsMarkerComponent>()
                .With<RectTransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < _itemsFilter.Length; i++)
            {
                var entity = _itemsFilter.GetEntity(i);
                ref var itemTransform = ref entity.GetComponent<RectTransformComponent>().RectTransform;

                var targetEntity = entity.GetComponent<MergeItemsMarkerComponent>().TargetItemEntity;
                ref var targetTransform = ref targetEntity.GetComponent<RectTransformComponent>().RectTransform;
                
                targetEntity.GetComponent<ItemComponent>().Score += 1;
                LayerCorrection(itemTransform, targetTransform);

                itemTransform.DOAnchorPos(targetTransform.anchoredPosition, 0.1f).OnComplete(() =>
                {
                    entity.AddComponent<RemoveThisEntityMarkerComponent>();
                });
                
                entity.RemoveComponent<MergeItemsMarkerComponent>();
            }
        }
        
        private void LayerCorrection(RectTransform itemTransform, RectTransform targetTransform)
        {
            var itemIndex = itemTransform.GetSiblingIndex();
            var targetIndex = targetTransform.GetSiblingIndex();

            if (itemIndex > targetIndex)
            {
                targetTransform.SetSiblingIndex(itemIndex);
                itemTransform.SetSiblingIndex(targetIndex);
            }
        }
    }
}