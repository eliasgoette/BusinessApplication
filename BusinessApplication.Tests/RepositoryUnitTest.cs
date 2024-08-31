using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using Microsoft.EntityFrameworkCore;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class RepositoryUnitTest
    {
        private List<Address> _addresses;
        private List<Customer> _customers;
        private DbContextFactoryMethod _dbContextFactory;
        private IRepository<Customer> _customerRepository;
        private IRepository<Address> _addressRepository;

        [TestInitialize]
        public void Setup()
        {
            _dbContextFactory = () =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseInMemoryDatabase("Repository Test DB");
                return new AppDbContext(optionsBuilder.Options);
            };

            SeedTestData();

            var logger = new Logger();

            _customerRepository = new Repository<Customer>(_dbContextFactory, logger);
            _addressRepository = new Repository<Address>(_dbContextFactory, logger);
        }

        private void SeedTestData()
        {
            using (var dbContext = _dbContextFactory.Invoke())
            {
                _addresses = new List<Address>
                {
                    new Address {
                        Country = "United States",
                        ZipCode = "IN 47374",
                        City = "Richmond",
                        StreetAddress = "2739 Sugarfoot Lane"
                    },
                    new Address {
                        Country = "United States",
                        ZipCode = "CA 94103",
                        City = "San Francisco",
                        StreetAddress = "1234 Market Street"
                    },
                    new Address {
                        Country = "United States",
                        ZipCode = "NY 10001",
                        City = "New York",
                        StreetAddress = "5678 8th Avenue"
                    },
                    new Address {
                        Country = "Switzerland",
                        ZipCode = "8001",
                        City = "Zurich",
                        StreetAddress = "Bahnhofstrasse 10"
                    }
                };

                _customers = new List<Customer>
                {
                    new Customer
                    {
                        CustomerNumber = "CU-TEST-00001",
                        FirstName = "Jim",
                        LastName = "Ricketts",
                        Email = "JimBRicketts@yahoo.com",
                        CustomerAddress = _addresses[0]
                    },
                    new Customer
                    {
                        CustomerNumber = "CU-TEST-00002",
                        FirstName = "Jane",
                        LastName = "Doe",
                        Email = "JaneDoe@gmail.com",
                        CustomerAddress = _addresses[1]
                    },
                    new Customer
                    {
                        CustomerNumber = "CU-TEST-00003",
                        FirstName = "John",
                        LastName = "Smith",
                        Email = "JohnSmith@outlook.com",
                        CustomerAddress = _addresses[2]
                    },
                    new Customer
                    {
                        CustomerNumber = "CU-TEST-00004",
                        FirstName = "Emily",
                        LastName = "Brown",
                        Email = "EmilyBrown@gmail.com",
                        CustomerAddress = _addresses[3]
                    }
                };

                dbContext.AddRange(_addresses);
                dbContext.AddRange(_customers);
                dbContext.SaveChanges();
            }
        }

        [TestMethod]
        public void TestGetAll()
        {
            var customers = _customerRepository.GetAll().ToList();

            for (int i = 0; i < _customers.Count; i++)
            {
                Assert.AreEqual(_customers[i]?.Id, customers[i].Id);
            }
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
                CustomerNumber = "CU-TEST-ADD-" + Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe"
            };

            var result = await _customerRepository.AddAsync(customer);
            Assert.IsTrue(result);

            using (var context = _dbContextFactory.Invoke())
            {
                var customerFound = context.Set<Customer>().Where(x => x.CustomerNumber == customer.CustomerNumber).FirstOrDefault();
                Assert.AreEqual(customer.CustomerNumber, customerFound?.CustomerNumber);
            }
        }

        [TestMethod]
        public void TestUpdateCustomer()
        {
            using (var context = _dbContextFactory.Invoke())
            {
                var customer = context.Set<Customer>().FirstOrDefault();

                if (customer == null)
                {
                    Assert.Fail("No customer found to update.");
                }

                var testName = "Test" + Guid.NewGuid();
                customer.FirstName = testName;
                customer.LastName = testName;

                _customerRepository.Update(customer);

                var customerFound = context.Set<Customer>().Where(x => x.Id == customer.Id).FirstOrDefault();

                Assert.IsNotNull(customerFound);
                Assert.AreEqual(testName, customerFound?.FirstName);
                Assert.AreEqual(testName, customerFound?.LastName);
            }
        }

        [TestMethod]
        public void TestRemoveCustomer()
        {
            using (var context = _dbContextFactory.Invoke())
            {
                var customer = context.Set<Customer>().FirstOrDefault();
                Assert.IsNotNull(customer);

                var result = _customerRepository.Remove(customer);
                Assert.IsTrue(result);

                var customerFound = context.Set<Customer>().Where(x => x.Id == customer.Id).FirstOrDefault();
                Assert.IsNull(customerFound);
            }
        }
    }
}
