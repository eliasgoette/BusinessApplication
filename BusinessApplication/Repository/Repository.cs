using BusinessApplicationProject;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace BusinessApplication.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(T entity)
        {
            try
            {
                var canConnect = _context.Database.CanConnect();
                if (canConnect)
                {
                    _context.Set<T>().Add(entity);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    // TODO: Create error window
                }
            }
            catch (OperationCanceledException ex)
            {
                // TODO: Create error window
            }
            catch (DbUpdateException ex)
            {
                // TODO: Create error window
            }
            catch (DBConcurrencyException ex)
            {
                // TODO: Create error window
            }
            catch (Exception)
            {
                // TODO: Create error window
            }

            return false;
        }

        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect)
                {
                    await _context.Set<T>().AddAsync(entity);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    // TODO: Create error window
                }
            }
            catch (OperationCanceledException ex)
            {
                // TODO: Create error window
            }
            catch (DbUpdateException ex)
            {
                // TODO: Create error window
            }
            catch (DBConcurrencyException ex)
            {
                // TODO: Create error window
            }
            catch (Exception)
            {
                // TODO: Create error window
            }

            return false;
        }

        public IEnumerable<T> GetAllWhereAsOf(Expression<Func<T, bool>> predicate, DateTime utcDateTime)
        {
            try
            {
                var canConnect = _context.Database.CanConnect();
                if (canConnect)
                {
                    return _context.Set<T>().TemporalAsOf(utcDateTime).Where(predicate);
                }
                else
                {
                    // TODO: Create error window
                }
            }
            catch (OperationCanceledException ex)
            {
                // TODO: Create error window
            }
            catch (DbUpdateException ex)
            {
                // TODO: Create error window
            }
            catch (DBConcurrencyException ex)
            {
                // TODO: Create error window
            }
            catch (Exception)
            {
                // TODO: Create error window
            }

            return Enumerable.Empty<T>();
        }

        public IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> predicate)
        {
            return GetAllWhereAsOf(predicate, DateTime.UtcNow);
        }

        public IEnumerable<T> GetAll()
        {
            return GetAllWhere(x => true);
        }

        public bool Remove(T entity)
        {
            try
            {
                var canConnect = _context.Database.CanConnect();
                if (canConnect)
                {
                    _context.Set<T>().Remove(entity);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    // TODO: Create error window
                }
            }
            catch (DbUpdateException ex)
            {
                // TODO: Create error window
            }
            catch (DBConcurrencyException ex)
            {
                // TODO: Create error window
            }
            catch (Exception)
            {
                // TODO: Create error window
            }

            return false;
        }

        public bool Update(T entity)
        {
            try
            {
                var canConnect = _context.Database.CanConnect();
                if (canConnect)
                {
                    _context.Set<T>().Update(entity);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    // TODO: Create error window
                }
            }
            catch (DbUpdateException ex)
            {
                // TODO: Create error window
            }
            catch (DBConcurrencyException ex)
            {
                // TODO: Create error window
            }
            catch (Exception)
            {
                // TODO: Create error window
            }

            return false;
        }
    }
}
