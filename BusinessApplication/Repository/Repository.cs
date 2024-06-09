using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace BusinessApplication.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContextFactoryMethod _getDbContext;
        private readonly ILogger _logger;

        public delegate DbContext DbContextFactoryMethod();

        public Repository(DbContextFactoryMethod getDbContext, ILogger logger)
        {
            _getDbContext = getDbContext;
            _logger = logger;
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
                        LogConnectionError();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }

        public List<T> GetAllWhereAsOf(Expression<Func<T, bool>> predicate, DateTime utcDateTime)
        {
            try
            {
                using (var context = _getDbContext())
                {
                    var canConnect = context.Database.CanConnect();
                    if (canConnect)
                    {
                        var navigationProperties = typeof(T)
                            .GetProperties()
                            .Where(prop => prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                            .Select(prop => prop.Name)
                            .ToArray();

                        var query = context.Set<T>().TemporalAsOf(utcDateTime).Where(predicate);

                        foreach (var property in navigationProperties)
                        {
                            query = query.Include(property);
                        }

                        List<T> l = [.. query];

                        return l;
                    }
                    else
                    {
                        LogConnectionError();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return [];
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
                        LogConnectionError();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                        LogConnectionError();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }

        public void LogConnectionError()
        {
            _logger.LogError("Couldn't connect to DB.");
        }
    }
}
