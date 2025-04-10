using Domain;
using Infrastructure.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly RepositoryDbContext _dbContext;
        public RepositoryBase(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
             _dbContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }
    }
}
