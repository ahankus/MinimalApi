using MinimalApi.Models;

namespace MinimalApi.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<CustomerModel> GetCustomers();
        CustomerModel GetCustomerById(string id);
        void AddCustomer(CustomerModel customer);
        bool DeleteCustomer(string id);
        bool DeleteAll();
    }

}
