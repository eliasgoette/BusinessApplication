using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using Moq;

namespace BusinessApplication.Tests
{
    [TestClass]
    public class CustomerViewModelUnitTest
    {
        private Mock<IRepository<Customer>> _mockCustomerRepository;
        private Mock<IRepository<Address>> _mockAddressRepository;
        private Mock<ILogger> _mockLogger;
        private CustomerViewModel _viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockCustomerRepository = new Mock<IRepository<Customer>>();
            _mockAddressRepository = new Mock<IRepository<Address>>();
            _mockLogger = new Mock<ILogger>();

            _viewModel = new CustomerViewModel(_mockCustomerRepository.Object, _mockAddressRepository.Object, _mockLogger.Object);
        }

        [TestMethod]
        public void CustomerViewModel_InitializesCorrectly()
        {
            // Arrange
            var customers = new List<Customer> {
                new Customer {
                    CustomerNumber = "CU12345",
                    CustomerAddress = new Address {
                        Country = "",
                        ZipCode = "",
                        City = "",
                        StreetAddress = ""
                    }
                }
            };
            _mockCustomerRepository.Setup(repo => repo.GetAll()).Returns(customers.AsQueryable());

            // Act
            _viewModel = new CustomerViewModel(_mockCustomerRepository.Object, _mockAddressRepository.Object, _mockLogger.Object);

            // Assert
            Assert.IsNotNull(_viewModel.SearchResults);
            Assert.AreEqual(1, _viewModel.SearchResults.Count);
            Assert.AreEqual("CU12345", _viewModel.SearchResults[0].CustomerNumber);
        }

        [TestMethod]
        public void ExecuteSearch_SearchesCorrectly()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer {
                    CustomerNumber = "CU12345", CustomerAddress = new Address {
                        Country = "",
                        ZipCode = "",
                        City = "",
                        StreetAddress = ""
                    }
                },
                new Customer {
                    CustomerNumber = "CU67890", CustomerAddress = new Address {
                        Country = "",
                        ZipCode = "",
                        City = "",
                        StreetAddress = ""
                    }
                }
            };
            _mockCustomerRepository.Setup(repo => repo.GetAllWhere(It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, bool>>>()))
                                   .Returns((System.Linq.Expressions.Expression<System.Func<Customer, bool>> predicate) =>
                                       customers.AsQueryable().Where(predicate));

            // Act
            _viewModel.SearchCustomerNumber = "CU123";
            _viewModel.Search.Execute(this);

            // Assert
            Assert.AreEqual(1, _viewModel.SearchResults.Count);
            Assert.AreEqual("CU12345", _viewModel.SearchResults[0].CustomerNumber);
        }

        [TestMethod]
        public void ExecuteAdd_AddsCustomerCorrectly()
        {
            // Arrange
            _viewModel.CustomerNumber = "CU12345";
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.Email = "john.doe@example.com";
            _viewModel.Website = "http://example.com";
            _viewModel.PasswordHash = "Password123";
            _viewModel.CustomerAddressCountry = "USA";
            _viewModel.CustomerAddressZipCode = "12345";
            _viewModel.CustomerAddressCity = "New York";
            _viewModel.CustomerAddressStreetAddress = "123 Main St";

            // Act
            _viewModel.Add.Execute(this);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.AddAsync(It.Is<Customer>(c =>
                c.CustomerNumber == "CU12345" &&
                c.FirstName == "John" &&
                c.LastName == "Doe" &&
                c.Email == "john.doe@example.com" &&
                c.Website == "http://example.com" &&
                c.PasswordHash == "Password123" &&
                c.CustomerAddress.Country == "USA" &&
                c.CustomerAddress.ZipCode == "12345" &&
                c.CustomerAddress.City == "New York" &&
                c.CustomerAddress.StreetAddress == "123 Main St"
            )), Times.Once);

            _mockCustomerRepository.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
        }

        [TestMethod]
        public void ExecuteUpdate_UpdatesCustomerCorrectly()
        {
            // Arrange
            var cust = new Customer
            {
                CustomerNumber = "CU12345",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Website = "http://example.com",
                PasswordHash = "Password123",
                CustomerAddress = new Address
                {
                    Country = "USA",
                    ZipCode = "12345",
                    City = "New York",
                    StreetAddress = "123 Main St"
                }
            };

            _viewModel.SelectedCustomer = cust;

            // Act
            _viewModel.Update.Execute(this);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.Update(It.Is<Customer>(c => c == cust)), Times.Once);

            _mockCustomerRepository.Verify(repo => repo.Update(It.IsAny<Customer>()), Times.Once);
        }

        [TestMethod]
        public void ExecuteRemove_RemovesCustomerCorrectly()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerNumber = "CU12345",
                CustomerAddress = new Address { Country = "USA", ZipCode = "12345", City = "New York", StreetAddress = "123 Main St" }
            };
            _viewModel.SelectedCustomer = customer;

            // Act
            _viewModel.Remove.Execute(this);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.Remove(It.Is<Customer>(c => c.CustomerNumber == "CU12345")), Times.Once);
            _mockAddressRepository.Verify(repo => repo.Remove(It.Is<Address>(a => a.StreetAddress == "123 Main St")), Times.Once);
        }

        [TestMethod]
        public void ValidateCustomerNumber_ValidCustomerNumber_ReturnsTrue()
        {
            // Arrange
            _viewModel.CustomerNumber = "CU12345";
            var method = typeof(CustomerViewModel).GetMethod("ValidateCustomerNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = (bool)method.Invoke(_viewModel, null);

            // Assert
            Assert.IsTrue(result);
        }

        public void ValidateWebsite_ValidWebsite_ReturnsTrue()
        {
            // Arrange
            _viewModel.Website = "http://example.com";
            var method = typeof(CustomerViewModel).GetMethod("ValidateWebsite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = (bool)method.Invoke(_viewModel, null);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            _viewModel.Email = "test@example.com";
            var method = typeof(CustomerViewModel).GetMethod("ValidateEmail", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = (bool)method.Invoke(_viewModel, null);

            // Assert
            Assert.IsTrue(result);


        }

        [TestMethod]
        public void ValidatePassword_ValidPassword_ReturnsTrue()
        {
            // Arrange
            _viewModel.NewPassword = "Valid1Password";
            var method = typeof(CustomerViewModel).GetMethod("ValidateNewPassword", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = (bool)method.Invoke(_viewModel, null);

            // Assert
            Assert.IsTrue(result);
        }

    }
}