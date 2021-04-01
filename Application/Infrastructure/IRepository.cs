namespace Application.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
    }
}