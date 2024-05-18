using System.Linq.Expressions;

namespace BusinessApplication.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> searchTerm);
        Task<bool> AddAsync(T entity);
        bool Remove(T entity);
        bool Update(T entity);
    }
}
