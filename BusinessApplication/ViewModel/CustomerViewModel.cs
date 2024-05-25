using BusinessApplication.Model;
using BusinessApplication.Repository;

namespace BusinessApplication.ViewModel
{
    public class CustomerViewModel
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerViewModel(IRepository<Customer> repository)
        {
            _customerRepository = repository;
        }
    }
}
