using System;
using System.Collections.Generic;

namespace Moonity.Core.Components
{
    public sealed class ComponentStorage
    {
        private readonly Dictionary<Type, IComponent> _core = new();
        private readonly Dictionary<Type, object> _platform = new();

        public void Add<T>(T component) where T : class, IComponent
        {
            _core[typeof(T)] = component;
        }

        public T Get<T>() where T : class, IComponent
        {
            if (_core.TryGetValue(typeof(T), out var comp))
                return comp as T;

            return null;
        }

        public bool TryGet<T>(out T component) where T : class, IComponent
        {
            if (_core.TryGetValue(typeof(T), out var comp))
            {
                component = comp as T;
                return component != null;
            }

            component = null;
            return false;
        }

        public bool Remove<T>() where T : IComponent
        {
            return _core.Remove(typeof(T));
        }

        public bool Has<T>() where T : IComponent
        {
            return _core.ContainsKey(typeof(T));
        }

        public void AddPlatform<T>(T component) where T : class
        {
            _platform[typeof(T)] = component;
        }

        public T GetPlatform<T>() where T : class
        {
            if (_platform.TryGetValue(typeof(T), out var comp))
                return comp as T;

            return null;
        }

        public bool HasPlatform<T>() where T : class
        {
            return _platform.ContainsKey(typeof(T));
        }

        public void RemovePlatform<T>() where T : class
        {
            _platform.Remove(typeof(T));
        }

        public void Clear()
        {
            _core.Clear();
            _platform.Clear();
        }
    }
}