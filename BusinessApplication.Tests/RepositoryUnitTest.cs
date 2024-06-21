using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using Microsoft.EntityFrameworkCore;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class RepositoryUnitTest
    {
        Repository<Customer> customerRepository;
        Repository<Address> addressRepository;

        [TestInitialize]
        public void Setup()
        {
            var addresses = new List<Address> {
                new Address {
                    Country = "United States",
                    ZipCode = "IN 47374",
                    City = "Richmond",
                    StreetAddress = "2739 Sugarfoot Lane"
                }
            };

            var customers = new List<Customer>
            {
                new Customer
                {
                    CustomerNumber = "CU-TEST-00001",
                    FirstName = "Jim",
                    LastName = "Ricketts",
                    Email = "JimBRicketts@yahoo.com",
                    CustomerAddress = addresses[0]
                }
            };

            var logger = new Logger();
            customerRepository = new Repository<Customer>(() => new AppDbContextStub(), logger);
            addressRepository = new Repository<Address>(() => new AppDbContextStub(), logger);
        }

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
        }

        [TestMethod]
        public void TestUpdateCustomer()
        {
            var customer = customerRepository.GetAll().FirstOrDefault();
            var testName = "Test";

            if (customer == null)
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
        public void TestRemoveCustomer()
        {
            var customer = customerRepository.GetAll().FirstOrDefault();

            Assert.IsNotNull(customer);

            Assert.IsTrue(customerRepository.Remove(customer));

            Assert.IsNull(customerRepository.GetAllWhere(x => x.Id == customer.Id).FirstOrDefault());
        }
    }

    public class AppDbContextStub : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}