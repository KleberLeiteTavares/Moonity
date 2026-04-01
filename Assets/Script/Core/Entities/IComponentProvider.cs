namespace Moonity.Core.Entities
{
    public interface IComponentProvider
    {
        void Add<T>();
        T Get<T>() where T : class;
        void Clear();
    }
}
