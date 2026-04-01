using System;
using System.Collections.Generic;

using Moonity.Core.Entities;

using UnityEngine;

namespace Moonity.Core.Unity.Entities
{
    public sealed class GameObjectComponentProvider : IComponentProvider
    {
        private readonly GameObject _gameObject;
        private readonly Dictionary<Type, object> _cache = new();

        public GameObjectComponentProvider(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public void Add<T>()
        {
            var type = typeof(T);

            if (_cache.ContainsKey(type))
                return;

            var component = _gameObject.AddComponent(type);
            _cache[type] = component;
        }

        public T Get<T>() where T : class
        {
            if (_cache.TryGetValue(typeof(T), out var obj))
                return obj as T;

            return null;
        }

        public void Clear()
        {
            foreach (var type in _cache.Keys)
            {
                var comp = _gameObject.GetComponent(type);
                if (comp != null)
                    UnityEngine.Object.Destroy(comp);
            }

            _cache.Clear();
        }
    }
}