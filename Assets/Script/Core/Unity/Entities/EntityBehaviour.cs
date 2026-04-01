using Moonity.Core.Entities;

using UnityEngine;

namespace Moonity.Core.Unity.Entities
{
    public sealed class EntityBehaviour : MonoBehaviour
    {
        public Entity Entity { get; private set; }

        private void Awake()
        {
            Entity = new Entity(new GameObjectComponentProvider(gameObject));
        }
    }
}