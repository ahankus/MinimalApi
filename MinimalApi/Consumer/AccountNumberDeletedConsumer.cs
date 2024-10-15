using MassTransit;
using MinimalApi.Interfaces;

namespace MinimalApi.Consumer;

public class AccountNumberDeletedConsumer : IConsumer<AccountNumberDeleted>
{
    private readonly ICustomerRepository _customerRepository;

    public AccountNumberDeletedConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Consume(ConsumeContext<AccountNumberDeleted> context)
    {
        string accountNumberId = context.Message.AccountNumberId;

        var customersToDelete = _customerRepository.GetAll()
            .Where(x => x.AccountNumber.Id == accountNumberId)
            .ToList();

        // Delete associated customers
        foreach (var customer in customersToDelete)
        {
            _customerRepository.Delete(customer.Id);
        }
    }

}