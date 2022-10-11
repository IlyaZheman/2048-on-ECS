using Morpeh;

namespace Base
{
    public abstract class InitializerBase : IInitializer
    {
        public World World { get; set; }

        public abstract void OnAwake();

        public virtual void Dispose() { }
    }
}