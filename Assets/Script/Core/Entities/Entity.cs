using Moonity.Core.Components;

namespace Moonity.Core.Entities
{
    public sealed class Entity
    {
        public EntityId Id { get; private set; }

        private readonly ComponentStorage _components = new();
        private readonly IComponentProvider _provider;

        public Entity(IComponentProvider provider)
        {
            _provider = provider;
        }

        public void Setup(EntityId id)
        {
            Id = id;

            _components.Clear();
            _provider?.Clear();
        }

        public void NextGeneration()
        {
            Id = new EntityId(Id.RawId, Id.Generation + 1);

            _components.Clear();
            _provider?.Clear();
        }

        public void AddComponent<T>(T component) where T : class, IComponent
            => _components.Add(component);

        public T GetComponent<T>() where T : class, IComponent
            => _components.Get<T>();

        public void AddPlatformComponent<T>() where T : class
        {
            _provider?.Add<T>();

            var instance = _provider?.Get<T>();
            if (instance != null)
                _components.AddPlatform(instance);
        }

        public T GetPlatformComponent<T>() where T : class
            => _components.GetPlatform<T>();
    }
}