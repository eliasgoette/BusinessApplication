using BusinessApplication.Model;
using BusinessApplicationProject;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class AppDBContextUnitTest
    {
        [TestMethod]
        public void TestAddCustomer()
        {
            // Shouldn't always run => uncomment if needed

            var addr = new Address
            {
                Country = "United States",
                ZipCode = "10001",
                City = "New York City",
                StreetAddress = "101 5th Ave"
            };

            var cust = new Customer
            {
                CustomerAddress = addr,
                CustomerNumber = "CU-00001",
                FirstName = "John",
                LastName = "Doe"
            };

            using (var ctx = new AppDbContext())
            {
                ctx.Add(cust);
                ctx.SaveChanges();

                Assert.IsNotNull(ctx.Customers.FirstOrDefault());
            }
        }

        [TestMethod]
        public void TestAddMultipleCustomers()
        {
            // Shouldn't always run => uncomment if needed

            var customers = new List<Customer>();

            for (int i = 0; i < 5; i++)
            {
                var addr = new Address
                {
                    Country = "United States",
                    ZipCode = "10001",
                    City = "New York City",
                    StreetAddress = $"20{i} 6th Ave"
                };

                var cust = new Customer
                {
                    CustomerAddress = addr,
                    CustomerNumber = $"CU-0001{i}",
                    FirstName = "Max",
                    LastName = "Muster"
                };

                customers.Add(cust);
            }

            using (var ctx = new AppDbContext())
            {
                foreach (var customer in customers)
                {
                    ctx.Add(customer);
                    ctx.SaveChanges();

                    Assert.IsNotNull(ctx.Customers.Where(c => c.CustomerNumber == customer.CustomerNumber).FirstOrDefault());
                    Assert.IsNotNull(ctx.Customers.Where(c => c.CustomerNumber == customer.CustomerNumber).FirstOrDefault()?.CustomerAddress);
                }
            }
        }

        [TestMethod]
        public void TestAlterCustomer()
        {
            // Shouldn't always run => uncomment if needed
            // This code will update the first customer from the database

            using (var ctx = new AppDbContext())
            {
                var customer = ctx.Customers.FirstOrDefault();
                var testPrefix = "UPDATE_TEST123";

                if (customer != null)
                {
                    customer.Email = testPrefix + customer.Email;
                    ctx.Update(customer);

                    ctx.SaveChanges();

                    Assert.AreEqual(testPrefix, ctx.Customers.FirstOrDefault()?.Email?.Substring(0, testPrefix.Length));
                }
                else
                {
                    Assert.Fail("Customer is null.");
                }
            }
        }

        [TestMethod]
        public void TestDeleteAllCustomers()
        {
            // Shouldn't always run => uncomment if needed
            // This code will permanently remove all customer data from the database

            using (var ctx = new AppDbContext())
            {
                var customers = ctx.Customers.ToList();
                var initialCustomers = customers.Count();
                var initialAddresses = ctx.Addresses.Count();

                foreach (var customer in customers)
                {
                    //ctx.Remove(customer);
                    //ctx.Remove(customer.CustomerAddress);
                    ctx.Set<Address>().Remove(customer.CustomerAddress);
                    ctx.Set<Customer>().Remove(customer);
                }

                ctx.SaveChanges();

                var customersLeft = ctx.Customers.Count();
                var addressesLeft = ctx.Addresses.Count();

                Assert.AreEqual(0, customersLeft);
                Assert.AreEqual(initialAddresses - initialCustomers, addressesLeft);
            }
        }
    }
}