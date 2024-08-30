using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using Microsoft.EntityFrameworkCore;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class RepositoryUnitTest
    {
        private IRepository<Customer> customerRepository;
        private IRepository<Address> addressRepository;
        private AppDbContext dbContext;

        [TestInitialize]
        public void Setup()
        {
            //var options = new DbContextOptionsBuilder<AppDbContext>()
            //    .UseInMemoryDatabase(databaseName: "BA Project Test DB")
            //    .Options;

            //dbContext = new AppDbContext(options);

            SeedTestData();

            var logger = new Logger();

            customerRepository = new Repository<Customer>(() => dbContext, logger);
            addressRepository = new Repository<Address>(() => dbContext, logger);
        }

        private void SeedTestData()
        {
            var addresses = new List<Address>
            {
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

            dbContext.AddRange(addresses);
            dbContext.AddRange(customers);
            dbContext.SaveChanges();
        }

        [TestMethod]
        public async Task TestAddCustomer()
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

            var result = await customerRepository.AddAsync(customer);
            Assert.IsTrue(result);

            var allC = customerRepository.GetAll();
            var customerFound = customerRepository.GetAllWhere(x => x.CustomerNumber == customer.CustomerNumber).FirstOrDefault();
            Assert.IsNotNull(customerFound);
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
            Assert.IsNotNull(customerFound);
            Assert.AreEqual(testName, customerFound?.FirstName);
            Assert.AreEqual(testName, customerFound?.LastName);
        }

        [TestMethod]
        public void TestRemoveCustomer()
        {
            var customer = customerRepository.GetAll().FirstOrDefault();

            Assert.IsNotNull(customer);

            var result = customerRepository.Remove(customer);
            Assert.IsTrue(result);

            var customerFound = customerRepository.GetAllWhere(x => x.Id == customer.Id).FirstOrDefault();
            Assert.IsNull(customerFound);
        }
    }
}
