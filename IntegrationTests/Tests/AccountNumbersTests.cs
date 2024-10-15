using FluentAssertions;
using IntegrationTests.Helpers;
using IntegrationTests.TestFixtures;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Interfaces;
using MinimalApi.Models;

namespace IntegrationTests.Tests;

public class AccountNumbersTests : IAsyncLifetime, IClassFixture<MinimalApiWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly IAccountNumberRepository _accountNumberRepository;
    private readonly ICustomerRepository _customerRepository;
    private AccountNumberModel? _account1;
    private AccountNumberModel? _account2;
    private CustomerModel _customer1;

    public AccountNumbersTests(MinimalApiWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        _accountNumberRepository = factory.Services.GetRequiredService<IAccountNumberRepository>();
        _customerRepository = factory.Services.GetRequiredService<ICustomerRepository>();
        UseProjectRelativeDirectory("Tests/VerifyFiles");
    }
    public async Task InitializeAsync()
    {
        _account1 = new AccountNumberModel { Number = "0123-4567-890123-0123-0123" };
        _accountNumberRepository.Add(_account1);
        _account2 = new AccountNumberModel { Number = "999-998-997-996" };
        _accountNumberRepository.Add(_account2);
        _customer1 = new CustomerModel { Name = "Tester", AccountNumber = _account1 };
        _customerRepository.Add(_customer1);
    }
    
    [Fact]
    public async Task GetAll_Test_Returns200_With_AccountNumbersList()
    {
        //Act
        var response = await _httpClient.GetAsync(ApiRouteHelper.AccountNumbers());
        
        //Assert
        response.Should().Be200Ok();
        await VerifyJson(response.Content.ReadAsStreamAsync()).IgnoreMember("id");
    }
    
    [Fact]
    public async Task Create_Test_Returns200_With_NewAccountNumber()
    {
        //Arrange
        var payload = new 
        {
            Number = "0000-9999-8888-7777"
        };
        
        //Act
        var response = await _httpClient.PostAsJsonAsync(ApiRouteHelper.AccountNumbers(), payload);
        
        //Assert
        response.Should().Be201Created();
        _accountNumberRepository.GetByNumber("0000-9999-8888-7777").Should().NotBeNull();
    }
    
    [Fact]
    public async Task Create_NumberAlreadyInDb_Test_Returns422_With_ErrorMessage()
    {
        //Arrange
        var payload = new 
        {
            Number = _account1.Number
        };
        
        //Act
        var response = await _httpClient.PostAsJsonAsync(ApiRouteHelper.AccountNumbers(), payload);
        
        //Assert
        response.Should().Be422UnprocessableEntity();
        await VerifyJson(response.Content.ReadAsStreamAsync());
    }
    
    [Fact]
    public async Task GetByName_Test_Returns200_With_AccountNumber()
    {
        //Act
        var response = await _httpClient.GetAsync(ApiRouteHelper.AccountNumberNumber(_account1.Number));
        
        //Assert
        response.Should().Be200Ok();
        await VerifyJson(response.Content.ReadAsStreamAsync()).IgnoreMember("id");
    }
    
    [Fact]
    public async Task Delete_Test_Returns204_WithAccountDeleted()
    {
        //Act
        var response = await _httpClient.DeleteAsync(ApiRouteHelper.AccountNumberNumber(_account2.Id));
        
        //Assert
        response.Should().Be204NoContent();
        _accountNumberRepository.GetByNumber(_account2.Number).Should().BeNull();
    }
    
    [Fact]
    public async Task Delete_Test_Returns204_WithAssociatedCustomerDeleted()
    {
        //_customer1 has _account1 assigned
        //Act
        var response = await _httpClient.DeleteAsync(ApiRouteHelper.AccountNumberNumber(_account1.Id));
        
        //Assert
        response.Should().Be204NoContent();
        
        //wait for consumer delete _customer1
        await Task.Delay(500);
        _customerRepository.GetById(_customer1.Id).Should().BeNull();
        _accountNumberRepository.GetByNumber(_account1.Number).Should().BeNull();
    }
    
    public async Task DisposeAsync()
    {
        _accountNumberRepository.DeleteAll();
    }
}