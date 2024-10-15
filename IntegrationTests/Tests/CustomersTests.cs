using FluentAssertions;
using IntegrationTests.Helpers;
using IntegrationTests.TestFixtures;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Interfaces;
using MinimalApi.Models;

namespace IntegrationTests.Tests;

public class CustomersTests : IAsyncLifetime, IClassFixture<MinimalApiWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly IAccountNumberRepository _accountNumberRepository;
    private AccountNumberModel? _account1;
    private readonly ICustomerRepository _customerRepository;
    private CustomerModel? _customer1;
    private CustomerModel? _customer2;

    public CustomersTests(MinimalApiWebApplicationFactory factory)
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
        
        _customer1 = new CustomerModel { Name = "Tester", AccountNumber = _account1 };
        _customerRepository.Add(_customer1);
        _customer2 = new CustomerModel { Name = "Test Customer", AccountNumber = _account1 };
        _customerRepository.Add(_customer2);
    }
    
    [Fact]
    public async Task GetAll_Test_Returns200_With_CustomersList()
    {
        //Act
        var response = await _httpClient.GetAsync(ApiRouteHelper.Customers());
        
        //Assert
        response.Should().Be200Ok();
        await VerifyJson(response.Content.ReadAsStreamAsync()).IgnoreMember("id");
    }
    
    [Fact]
    public async Task Create_Test_Returns200_With_NewCustomer()
    {
        //Arrange
        var payload = new 
        {
            Name = "New Customer",
            AccountNumber = _account1
        };
        
        //Act
        var response = await _httpClient.PostAsJsonAsync(ApiRouteHelper.Customers(), payload);
        
        //Assert
        response.Should().Be201Created();
    }
    
    [Fact]
    public async Task GetById_Test_Returns200_With_Customer()
    {
        //Act
        var response = await _httpClient.GetAsync(ApiRouteHelper.CustomerId(_customer2.Id));
        
        //Assert
        response.Should().Be200Ok();
        await VerifyJson(response.Content.ReadAsStreamAsync()).IgnoreMember("id");
    }
    
    [Fact]
    public async Task Delete_Test_Returns204_WithCustomerDeleted()
    {
        //Act
        var response = await _httpClient.DeleteAsync(ApiRouteHelper.CustomerId(_customer1.Id));
        
        //Assert
        response.Should().Be204NoContent();
        _customerRepository.GetById(_customer1.Id).Should().BeNull();
    }
    
    public async Task DisposeAsync()
    {
        _accountNumberRepository.DeleteAll();
        _customerRepository.DeleteAll();
    }
}