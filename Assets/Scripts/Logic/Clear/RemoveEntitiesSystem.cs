using Base;
using Data.Clear;
using Data.Global;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Logic.Clear
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoveEntitiesSystem : LateUpdateSystemBase
    {
        private Filter _withGameObjectFilter;
        private Filter _withoutGameObjectFilter;
        
        public override void OnAwake()
        {
            _withGameObjectFilter = World.Filter
                .With<RemoveThisEntityMarkerComponent>()
                .With<RectTransformComponent>();
            _withoutGameObjectFilter = World.Filter
                .With<RemoveThisEntityMarkerComponent>()
                .Without<RectTransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            RemoveEntitiesWithGameObject();
            RemoveEntitiesWithoutGameObject();
        }

        private void RemoveEntitiesWithGameObject()
        {
            for (var i = 0; i < _withGameObjectFilter.Length; i++)
            {
                var entity = _withGameObjectFilter.GetEntity(i);
                ref var transform = ref entity.GetComponent<RectTransformComponent>().RectTransform;
                Object.Destroy(transform.gameObject);
                World.RemoveEntity(entity);
            }
        }

        private void RemoveEntitiesWithoutGameObject()
        {
            for (var i = 0; i < _withoutGameObjectFilter.Length; i++)
            {
                var entity = _withGameObjectFilter.GetEntity(i);
                World.RemoveEntity(entity);
            }
        }
    }
}