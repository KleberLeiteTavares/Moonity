namespace Moonity.Core.Entities
{
    public readonly struct EntityId
    {
        public readonly long Value;

        public EntityId(long id, long generation)
        {
            Value = (generation << 32) | (id & 0xFFFFFFFF);
        }

        public long RawId => Value & 0xFFFFFFFF;
        public long Generation => Value >> 32;

        public override string ToString() => Value.ToString();
    }
}
