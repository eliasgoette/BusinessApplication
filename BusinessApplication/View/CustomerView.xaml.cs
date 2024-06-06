using BusinessApplication.Model;
using BusinessApplication.Repository;
using System.Linq.Expressions;
using System.Windows.Controls;

namespace BusinessApplication.View
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
            var customerRepository = new Repository<Customer>(() => new AppDbContext());
            //var customerRepository = new RepositoryStub<Customer>();
            DataContext = new CustomerViewModel(customerRepository);
        }
    }

    public class RepositoryStub<T> : IRepository<T> where T : class
    {
        private List<Customer> data = new List<Customer> {
            new Customer
            {
                CustomerAddress = new Address
                {
                    Country = "United States",
                    ZipCode = "NY 10001",
                    City = "Manhattan",
                    StreetAddress = "101 5th Avenue"
                },
                CustomerNumber = "CU-TEST-00010",
                FirstName = "John",
                LastName = "Doe"
            },
            new Customer
            {
                CustomerAddress = new Address
                {
                    Country = "United States",
                    ZipCode = "CA 3928",
                    City = "Los Angeles",
                    StreetAddress = "123 Mulholland Drive"
                },
                CustomerNumber = "CU-TEST-00020",
                FirstName = "Jane",
                LastName = "Doe"
            }
        };

        public async Task<bool> AddAsync(T entity)
        {
            var customer = entity as Customer;

            if (customer != null)
            {
                await Task.Delay(100);
                data.Add(customer);
            }

            return true;
        }

        public IEnumerable<T> GetAll()
        {
            return GetAllWhere(x => true);
        }

        public IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> searchTerm)
        {
            return GetAllWhereAsOf(searchTerm, DateTime.UtcNow);
        }

        public IEnumerable<T> GetAllWhereAsOf(Expression<Func<T, bool>> predicate, DateTime utcDateTime)
        {
            if (typeof(T) == typeof(Customer))
            {

                return data.OfType<T>().Where(predicate.Compile());
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        public bool Remove(T entity)
        {
            var customer = entity as Customer;
            if (customer != null)
            {
                data.Remove(customer);
            }
            return true;
        }

        public bool Update(T entity)
        {
            var customer = entity as Customer;
            if (customer != null)
            {
                var existingCustomer = data.FirstOrDefault(c => c.CustomerNumber == customer.CustomerNumber);
                if (existingCustomer != null)
                {
                    existingCustomer.CustomerAddress = customer.CustomerAddress;
                    existingCustomer.CustomerNumber = customer.CustomerNumber;
                    existingCustomer.FirstName = customer.FirstName;
                    existingCustomer.LastName = customer.LastName;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Website = customer.Website;
                    existingCustomer.PasswordHash = customer.PasswordHash;
                }
            }

            return true;
        }
    }
}
