using MinimalApi.Models;

namespace MinimalApi.Interfaces;

public interface ICustomerRepository
{
    CustomerModel GetById(string id);
    IEnumerable<CustomerModel> GetAll();
    void Add(CustomerModel customer);
    void Update(CustomerModel customer);
    void Delete(string id);
    void DeleteAll();
}