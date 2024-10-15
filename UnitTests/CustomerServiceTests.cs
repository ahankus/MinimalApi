using MinimalApi.Interfaces;
using MinimalApi.Models;
using MinimalApi.Services;
using NSubstitute;

namespace UnitTests
{
    public class CustomerServiceTests
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerRepository _customerRepository = Substitute.For<ICustomerRepository>();
        private CustomerModel? _customer;

        public CustomerServiceTests()
        {
            _customerService = new CustomerService(_customerRepository);
        }

        [SetUp]
        public void Setup()
        {
            _customer = new CustomerModel { Name = "Test", AccountNumber = new AccountNumberModel { Number = "0123-4567-890123-0123-0123" } };
        }

        [Test]
        public void GetById_ExistingCustomer_Returns_CorrectCustomer()
        {
            //Arrange
            _customerRepository.GetById(_customer.Id).Returns<CustomerModel>(_customer);

            //Act
            var customer = _customerService.GetCustomerById(_customer.Id);

            //Assert
            Assert.That(customer.Id, Is.EqualTo(_customer.Id));
            Assert.That(customer.Name, Is.EqualTo("Test"));
            Assert.That(customer.AccountNumber, Is.EqualTo(_customer.AccountNumber));
        }

        [Test]
        [TestCase("123")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("testId")]
        [TestCase("#$%^$#%^&$%^&#$%")]
        public void GetById_NotExistingCustomer_Returns_Null(string customerId)
        {
            //Arrange
            //_customerRepository.GetById(Arg.Any<string>()).ReturnsNull();
            _customerRepository.GetById(_customer.Id).Returns<CustomerModel>(_customer);

            //Act
            var customer = _customerService.GetCustomerById(customerId);

            //Assert
            Assert.IsNull(customer);
        }

        [TearDown]
        public void TearDown()
        {
            _customer = null;
        }
    }
}