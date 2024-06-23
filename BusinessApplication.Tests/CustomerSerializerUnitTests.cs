using BusinessApplication.Model;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class CustomerSerializerUnitTests
    {
        [TestMethod]
        public void TestToJson()
        {
            var customer = new Customer
            {
                CustomerAddress = new Address
                {
                    Id = 1,
                    Country = "United States",
                    City = "Idaho Springs",
                    ZipCode = "CO 80452",
                    StreetAddress = "1104 Miner St"
                },
                Id = 1,
                CustomerNumber = "CU-T-00001",
                FirstName = "John",
                LastName = "Smith"
            };

            var expectedJson = @"
                [
                    {
                        ""CustomerAddress"": {
                        ""Country"": ""United States"",
                        ""ZipCode"": ""CO 80452"",
                        ""City"": ""Idaho Springs"",
                        ""StreetAddress"": ""1104 Miner St""
                    },
                    ""CustomerNumber"": ""CU-T-00001"",
                    ""FirstName"": ""John"",
                    ""LastName"": ""Smith"",
                    ""Email"": null,
                    ""Website"": null,
                    ""PasswordHash"": null
                    }
                ]
            ";

            var json = CustomerSerializer.ToJson(new List<Customer> { customer });

            Assert.AreEqual(
                NormalizeJson(expectedJson),
                NormalizeJson(json)
            );
        }

        private string NormalizeJson(string json)
        {
            return json.Replace("\r\n", "").Replace(" ", "").Replace("\t", "");
        }
    }
}
