using Microsoft.AspNetCore.Mvc;
using MinimalApi.Interfaces;
using MinimalApi.Models;

namespace MinimalApi.Controllers;

[Route("/api/customers")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public IActionResult GetAllCustomers()
    {
        var customers = _customerRepository.GetAll();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public IActionResult GetCustomerById(string id)
    {
        var customer = _customerRepository.GetById(id);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpPost]
    public IActionResult AddCustomer([FromBody] CustomerModel customer)
    {
        _customerRepository.Add(customer);
        return CreatedAtAction(nameof(GetCustomerById), new
        {
            id = customer.Id
        }, customer);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCustomer(string id, [FromBody] CustomerModel customer)
    {
        if (id != customer.Id)
        {
            return BadRequest();
        }

        _customerRepository.Update(customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCustomer(string id)
    {
        var existingCustomer = _customerRepository.GetById(id);
        if (existingCustomer == null)
        {
            return NotFound();
        }

        _customerRepository.Delete(id);
        return NoContent();
    }
}