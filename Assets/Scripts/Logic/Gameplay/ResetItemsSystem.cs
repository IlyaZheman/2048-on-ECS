using Base;
using Data.Item;
using Morpeh;
using Morpeh.Globals;
using Services;
using Unity.IL2CPP.CompilerServices;

namespace Logic.Gameplay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ResetItemsSystem : UpdateSystemBase
    {
        private Filter _itemsFilter;

        private GlobalEventInt _sendMovement;

        public override void OnAwake()
        {
            _itemsFilter = World.Filter
                .With<ItemComponent>();

            _sendMovement = GlobalEventsProvider.Instance.SendMovement;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!_sendMovement)
            {
                return;
            }

            for (var i = 0; i < _itemsFilter.Length; i++)
            {
                var entity = _itemsFilter.GetEntity(i);

                ref var itemComponent = ref entity.GetComponent<ItemComponent>();
                itemComponent.HasMerged = true;
            }
        }
    }
}