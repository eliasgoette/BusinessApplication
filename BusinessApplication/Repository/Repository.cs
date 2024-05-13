using BusinessApplication.Model;
using BusinessApplicationProject;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessApplication.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext Context;

        public Repository(AppDbContext context)
        {
            Context = context;
        }

        public List<T> GetAll()
        {
            if (Context.Database.CanConnect())
            {
                var query = AddIncludes(typeof(T), Context.Set<T>());
                return query.ToList();
            }
            else
            {
                throw new TimeoutException();
            }
        }

        public List<T> Find(Expression<Func<T, bool>> condition)
        {
            if (Context.Database.CanConnect())
            {
                var query = AddIncludes(typeof(T), Context.Set<T>());
                return [.. query.Where(condition)];
            }
            else
            {
                throw new TimeoutException();
            }
        }

        public async Task AddAsync(T entity)
        {
            if (await Context.Database.CanConnectAsync())
            {
                await Context.Set<T>().AddAsync(entity);
                await Context.SaveChangesAsync();
            }
            else
            {
                throw new TimeoutException();
            }

        }

        public void Remove(T entity)
        {
            if (Context.Database.CanConnect())
            {
                Context.Set<T>().Remove(entity);
                Context.SaveChanges();
            }
            else
            {
                throw new TimeoutException();
            }
        }

        public void Update(T entity)
        {
            if (Context.Database.CanConnect())
            {
                Context.Set<T>().Update(entity);
                Context.SaveChanges();
            }
            else
            {
                throw new TimeoutException();
            }
        }

        private IQueryable<T> AddIncludes(Type type, IQueryable<T> query)
        {
            var includes = GetAllNavigationPropertyNames(typeof(T));

            foreach (var propertyName in includes)
            {
                query = query.Include(propertyName);
            }

            return query;
        }

        private List<string> GetAllNavigationPropertyNames(Type type)
        {
            var res = new List<string>();

            switch (type.Name)
            {
                case nameof(Article):
                    res.Add("Group.Parent");
                    break;

                case nameof(ArticleGroup):
                    res.Add("Parent");
                    break;

                case nameof(Customer):
                    res.Add("CustomerAddress");
                    break;

                case nameof(Invoice):
                    res.Add("BillingAddress");
                    res.Add("OrderInformations.CustomerDetails.CustomerAddress");
                    res.Add("OrderInformations.Positions.ArticleDetails.Group.Parent");
                    break;

                case nameof(Order):
                    res.Add("CustomerDetails.CustomerAddress");
                    res.Add("Positions.ArticleDetails.Group.Parent");
                    break;

                case nameof(Position):
                    res.Add("ArticleDetails.Group.Parent");
                    break;

                default:
                    break;
            }

            return res;
        }
    }
}