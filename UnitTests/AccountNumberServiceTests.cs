using MinimalApi.Interfaces;
using MinimalApi.Models;
using MinimalApi.Services;
using NSubstitute;

namespace UnitTests
{
    [TestFixture, Parallelizable(ParallelScope.Fixtures)]
    public class AccountNumberServiceTests
    {
        private readonly IAccountNumberService _accountNumberService;
        private readonly IAccountNumberRepository _accountNumberRepository = Substitute.For<IAccountNumberRepository>();
        private AccountNumberModel _accountNumber;

        public AccountNumberServiceTests()
        {
            _accountNumberService = new AccountNumberService(_accountNumberRepository);
        }

        [SetUp]
        public void Setup()
        {
            _accountNumber = new AccountNumberModel { Number = "1111-2222-3333-4444" };
        }

        [Test]
        public void GetAccountNumberByNumber_ReturnsCorrectAccountNumber()
        {
            //Arrange
            _accountNumberRepository.GetByNumber(_accountNumber.Number).Returns<AccountNumberModel>(_accountNumber);

            //Act
            var accountNumber = _accountNumberService.GetAccountNumberByNumber(_accountNumber.Number);

            //Assert
            Assert.That(accountNumber.Number, Is.EqualTo(_accountNumber.Number));
            Assert.That(accountNumber.Id, Is.EqualTo(_accountNumber.Id));
        }


        [TearDown]
        public void TearDown()
        {
            _accountNumber = null;
        }
    }
}
