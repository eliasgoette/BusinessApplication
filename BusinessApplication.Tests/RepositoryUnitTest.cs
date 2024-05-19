using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplicationProject;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class RepositoryUnitTest
    {
        Repository<Customer> customerRepository = new Repository<Customer>(new AppDbContext());
        Repository<Address> addressRepository = new Repository<Address>(new AppDbContext());

        [TestMethod]
        public async Task TestAddCustomerAndOrder()
        {
            var address = new Address
            {
                Country = "United States",
                ZipCode = "20500",
                City = "Washington, D.C.",
                StreetAddress = "1600 Pennsylvania Avenue"
            };

            var customer = new Customer
            {
                CustomerAddress = address,
                CustomerNumber = "CU-TEST-10000",
                FirstName = "John",
                LastName = "Doe"
            };

            Assert.IsTrue(await customerRepository.AddAsync(customer));

            var customerFound = customerRepository.GetAllWhere(x => x == customer).FirstOrDefault();
            Assert.AreEqual(customer.CustomerNumber, customerFound?.CustomerNumber);

            var order = new Order
            {
                CustomerDetails = customer,
                OrderNumber = "O-10000",
                Positions = new List<Position>()
            };

            var orderRepository = new Repository<Order>(new AppDbContext());
            Assert.IsTrue(await orderRepository.AddAsync(order));
        }

        [TestMethod]
        public void TestUpdateCustomer()
        {
            var customer = customerRepository.GetAll().FirstOrDefault();
            var testName = "Test";

            if(customer == null)
            {
                Assert.Fail("No customer found to update.");
            }

            customer.FirstName = testName;
            customer.LastName = testName;
            customerRepository.Update(customer);

            var customerFound = customerRepository.GetAllWhere(x => x.Id == customer.Id).FirstOrDefault();
            Assert.AreEqual(testName, customerFound?.FirstName);
            Assert.AreEqual(testName, customerFound?.LastName);
        }

        [TestMethod]
        public void TestZRemoveCustomer()
        {
            // Z in name makes it the last test to run

            var customer = customerRepository.GetAll().FirstOrDefault();

            Assert.IsNotNull(customer);

            Assert.IsTrue(customerRepository.Remove(customer));

            Assert.IsNull(customerRepository.GetAllWhere(x => x.Id == customer.Id).FirstOrDefault());
        }
    }
}
