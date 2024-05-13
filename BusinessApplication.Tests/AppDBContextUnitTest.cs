namespace BusinessApplication.Tests
{
    [TestClass]
    public class AppDBContextUnitTest
    {
        [TestMethod]
        public void TestAddCustomer()
        {
            // Shouldn't always run => uncomment if needed

            //var addr = new Address {
            //    Country = "United States", 
            //    ZipCode = "10001",
            //    City = "New York City", 
            //    StreetAddress = "101 5th Ave"
            //};

            //var cust = new Customer
            //{
            //    CustomerAddress = addr,
            //    CustomerNumber = "CU-00001",
            //    FirstName = "John",
            //    LastName = "Doe"
            //};

            //using (var ctx = new AppDbContext())
            //{
            //    ctx.Add(cust);
            //    ctx.SaveChanges();

            //    Assert.IsNotNull(ctx.Customers.FirstOrDefault());
            //}
        }

        [TestMethod]
        public void TestAddMultipleCustomers()
        {
            // Shouldn't always run => uncomment if needed

            //var customers = new List<Customer>();

            //for (int i = 0; i < 5; i++)
            //{
            //    var addr = new Address
            //    {
            //        Country = "United States",
            //        ZipCode = "10001",
            //        City = "New York City",
            //        StreetAddress = $"20{i} 5th Ave"
            //    };

            //    var cust = new Customer
            //    {
            //        CustomerAddress = addr,
            //        CustomerNumber = $"CU-0001{i}",
            //        FirstName = "Max",
            //        LastName = "Muster"
            //    };

            //    customers.Add(cust);
            //}

            //using (var ctx = new AppDbContext())
            //{
            //    foreach(var customer in customers)
            //    {
            //        ctx.Add(customer);
            //        ctx.SaveChanges();

            //        Assert.IsNotNull(ctx.Customers.Where(c => c.CustomerNumber == customer.CustomerNumber).FirstOrDefault());
            //        Assert.IsNotNull(ctx.Customers.Where(c => c.CustomerNumber == customer.CustomerNumber).FirstOrDefault()?.CustomerAddress);
            //    }
            //}
        }
    }
}