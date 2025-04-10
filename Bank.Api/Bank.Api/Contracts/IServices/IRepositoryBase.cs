using System.Linq.Expressions;

namespace Infrastructure.IServices
{
    public interface IRepositoryBase <T> where T : class
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
