using MinimalApi.Interfaces;
using MinimalApi.Models;

namespace MinimalApi.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public IEnumerable<CustomerModel> GetCustomers()
    {
        return _customerRepository.GetAll();
    }

    public CustomerModel GetCustomerById(string id)
    {
        var existingCustomer = _customerRepository.GetById(id);
        if (existingCustomer != null)
        {
            _customerRepository.GetById(id);
            return _customerRepository.GetById(existingCustomer.Id);
        }
        return null;
    }

    public void AddCustomer(CustomerModel customer)
    {
        _customerRepository.Add(customer);
    }

    public bool DeleteCustomer(string id)
    {
        var existingCustomer = _customerRepository.GetById(id);
        if (existingCustomer != null)
        {
            _customerRepository.Delete(id);
            return true;
        }
        return false;
    }

    public bool DeleteAll()
    {
        var allCustomers = _customerRepository.GetAll();
        if (allCustomers != null && allCustomers.Any())
        {
            foreach (var customer in allCustomers)
            {
                _customerRepository.Delete(customer.Id);
            }
            return true;
        }
        return false;
    }
}
