using BusinessApplication.Model;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class SerializerUnitTests
    {
        [TestMethod]
        public void TestAddressToJson()
        {
            var addresses = new List<Address> {
                new Address {
                    Id = 1,
                    Country = "United States",
                    City = "Idaho Springs",
                    ZipCode = "CO 80452",
                    StreetAddress = "1104 Miner St"
                }
            };

            var expectedJson = @"[
                {
                    ""Id"": 1,
                    ""Country"": ""United States"",
                    ""City"": ""Idaho Springs"",
                    ""ZipCode"": ""CO 80452"",
                    ""StreetAddress"": ""1104 Miner St""
                }
            ]";


            var json = CustomerSerializer.ToJson(addresses);


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
