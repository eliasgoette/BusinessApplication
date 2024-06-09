using System.Linq.Expressions;

namespace BusinessApplication.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<bool> AddAsync(T entity);
        List<T> GetAllWhereAsOf(Expression<Func<T, bool>> predicate, DateTime utcDateTime);
        IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> searchTerm);
        IEnumerable<T> GetAll();
        bool Remove(T entity);
        bool Update(T entity);
    }
}
