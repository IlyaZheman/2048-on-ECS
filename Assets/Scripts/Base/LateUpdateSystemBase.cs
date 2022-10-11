using Morpeh;

namespace Base
{
    public abstract class LateUpdateSystemBase : ILateSystem
    {
        public World World { get; set; }

        public abstract void OnAwake();

        public abstract void OnUpdate(float deltaTime);

        public virtual void Dispose() { }
    }
}