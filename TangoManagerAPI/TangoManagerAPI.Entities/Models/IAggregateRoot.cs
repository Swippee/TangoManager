namespace TangoManagerAPI.Entities.Models
{
    public interface IAggregateRoot<out T>
    {
        public T RootEntity { get;}
    }
}
