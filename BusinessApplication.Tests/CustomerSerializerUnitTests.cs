using BusinessApplication.Model;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class CustomerSerializerUnitTests
    {
        private Customer GetCustomer()
        {
            return new Customer
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
        }

        private string NormalizeSerializedString(string json)
        {
            return json.Replace("\r\n", "").Replace(" ", "").Replace("\t", "");
        }

        [TestMethod]
        public void TestToJson()
        {
            var customer = GetCustomer();

            var expectedJson = @"
                [
                    {
                        ""CustomerAddress"": {
                            ""Country"":""UnitedStates"",
                            ""ZipCode"":""CO80452"",
                            ""City"":""IdahoSprings"",
                            ""StreetAddress"":""1104MinerSt""
                        },
                        ""CustomerNumber"":""CU-T-00001"",
                        ""Name"":""JohnSmith"",
                        ""Email"":null,
                        ""Website"":null,
                        ""PasswordHash"":null
                    }
                ]
            ";

            var json = CustomerSerializer.ToJson(new List<Customer> { customer });

            Assert.AreEqual(
                NormalizeSerializedString(expectedJson),
                NormalizeSerializedString(json)
            );
        }

        [TestMethod]
        public void TestToXml()
        {
            var customer = GetCustomer();

            var expectedXml = @"
                <?xml version=""1.0"" encoding=""utf-16""?>
                <Customers xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                  <Customer CustomerNumber=""CU-T-00001"">
                    <CustomerAddress>
                      <Country>UnitedStates</Country>
                      <ZipCode>CO80452</ZipCode>
                      <City>IdahoSprings</City>
                      <StreetAddress>1104MinerSt</StreetAddress>
                    </CustomerAddress>
                    <Name>John Smith</Name>
                  </Customer>
                </Customers>
            ";

            var xml = CustomerSerializer.ToXml(new List<Customer> { customer });

            Assert.AreEqual(
                NormalizeSerializedString(expectedXml),
                NormalizeSerializedString(xml)
            );
        }
    }
}
