using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace BusinessApplication.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContextFactoryMethod _getDbContext;

        public delegate DbContext DbContextFactoryMethod();

        public Repository(DbContextFactoryMethod getDbContext)
        {
            _getDbContext = getDbContext;
        }

        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                using (var context = _getDbContext())
                {
                    var canConnect = await context.Database.CanConnectAsync();
                    if (canConnect)
                    {
                        await context.Set<T>().AddAsync(entity);
                        await context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        // TODO: Create error window
                    }
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
                using (var context = _getDbContext())
                {
                    var canConnect = context.Database.CanConnect();
                    if (canConnect)
                    {
                        return context.Set<T>().TemporalAsOf(utcDateTime).Where(predicate).ToList();
                    }
                    else
                    {
                        // TODO: Create error window
                    }
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
                using (var context = _getDbContext())
                {
                    var canConnect = context.Database.CanConnect();
                    if (canConnect)
                    {
                        context.Set<T>().Remove(entity);
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        // TODO: Create error window
                    }
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
                using (var context = _getDbContext())
                {
                    var canConnect = context.Database.CanConnect();
                    if (canConnect)
                    {
                        context.Set<T>().Update(entity);
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        // TODO: Create error window
                    }
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
