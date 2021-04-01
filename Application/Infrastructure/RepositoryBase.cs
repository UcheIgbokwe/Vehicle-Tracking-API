using Microsoft.EntityFrameworkCore;

namespace Application.Infrastructure
{
    public class RepositoryBase<T> : Disposable, IRepository<T> where T : class
    {
        private readonly DbContext _dataContext;
        private readonly DbSet<T> dbSet;
        public RepositoryBase(DbContext dataContext)
        {
            _dataContext = dataContext;
            dbSet = _dataContext.Set<T>();
        }

        public virtual T Add(T entity)
        {
            return dbSet.Add(entity).Entity;
        }
    }
}