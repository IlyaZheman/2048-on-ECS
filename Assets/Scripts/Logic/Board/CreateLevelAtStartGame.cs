using Base;
using Services;
using Unity.IL2CPP.CompilerServices;

namespace Logic.Board
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CreateLevelAtStartGame : InitializerBase
    {
        public override void OnAwake()
        {
            GlobalEventsProvider.Instance.CreateLevel.NextFrame();
        }
    }
}