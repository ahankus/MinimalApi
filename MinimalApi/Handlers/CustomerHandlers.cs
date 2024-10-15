using MinimalApi.Interfaces;
using MinimalApi.Models;
using MinimalApi.Services;

namespace MinimalApi.Handlers;

public class CustomerHandlers
{
    public static IResult GetCustomersHandler(ICustomerRepository customerRepository)
    {
        var customerService = new CustomerService(customerRepository);
        var customers = customerService.GetCustomers();
        return Results.Ok(customers);
    }
    
    public static IResult GetCutomerByIdHandler(string id, ICustomerRepository customerRepository)
    {
        var customerServiceService = new CustomerService(customerRepository);
        var accountNumber = customerServiceService.GetCustomerById(id);
        if (accountNumber != null )
        {
            return Results.Ok(accountNumber);
        }
        else
        {
            return Results.NotFound();
        }
    }

    public static IResult AddCustomerHandler(
        CustomerModel customer,
        ICustomerRepository customerRepository,
        IAccountNumberRepository accountNumberRepository)
    {
        var customerService = new CustomerService(customerRepository);
        var existingaccNumber = accountNumberRepository.GetByNumber(customer.AccountNumber.Number);
        if (existingaccNumber == null)
        {
            return Results.NotFound();
        }
        else
        {
            customerService.AddCustomer(customer);
            return Results.Created($"/api/customers/{customer.Id}", customer); 
        }
    }

    public static IResult DeleteCustomerHandler(string id, ICustomerRepository customerRepository)
    {
        var customerService = new CustomerService(customerRepository);
        if (customerService.DeleteCustomer(id))
        {
            return Results.NoContent();
        }
        else
        {
            return Results.NotFound();
        }
    }
    
    public static IResult DeleteAll(ICustomerRepository customerRepository)
    {
        var customerService = new CustomerService(customerRepository);
        customerService.DeleteAll();
        return Results.NoContent();
    }
}