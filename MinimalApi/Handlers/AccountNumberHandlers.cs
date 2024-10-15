using MassTransit;
using MinimalApi.Consumer;
using MinimalApi.Interfaces;
using MinimalApi.Models;
using MinimalApi.Services;

namespace MinimalApi.Handlers
{
    public class AccountNumberHandlers
    {
        public static IResult GetAccountNumbersHandler(IAccountNumberRepository accountNumberRepository)
        {
            var accountNumberService = new AccountNumberService(accountNumberRepository);
            var accountNumbers = accountNumberService.GetAccountNumbers();
            return Results.Ok(accountNumbers);
        }
        
        public static IResult GetAccountNumberByNumberHandler(string number, IAccountNumberRepository accountNumberRepository)
        {
            var accountNumberService = new AccountNumberService(accountNumberRepository);
            var accountNumber = accountNumberService.GetAccountNumberByNumber(number);
            if (accountNumber != null )
            {
                return Results.Ok(accountNumber);
            }
            else
            {
                return Results.NotFound();
            }
        }

        public static IResult AddAccountNumberHandler(AccountNumberModel accountNumber, IAccountNumberRepository accountNumberRepository)
        {
            var accountNumberService = new AccountNumberService(accountNumberRepository);
            var existingaccNumber = accountNumberRepository.GetByNumber(accountNumber.Number);
            if (existingaccNumber != null)
            {
                return Results.UnprocessableEntity("AccountNumber already exists");
            }
            else
            {
                accountNumberService.AddAccountNumber(accountNumber);
                return Results.Created($"/api/accountNumbers/{accountNumber.Id}", accountNumber);
            }
        }

        public static IResult DeleteAccountNumberHandler(string id, IAccountNumberRepository accountNumberRepository, IBusControl busControl)
        {
            var accountNumberService = new AccountNumberService(accountNumberRepository);
            if (accountNumberService.DeleteAccountNumber(id))
            {
                // Publish account number deletion message
                busControl.Publish(new AccountNumberDeleted { AccountNumberId = id });
                return Results.NoContent();
            }
            else
            {
                return Results.NotFound();
            }
        }

        
        public static IResult DeleteAll(IAccountNumberRepository accountNumberRepository)
        {
            var accountNumberService = new AccountNumberService(accountNumberRepository);
            accountNumberService.DeleteAll();
            return Results.NoContent();
        }
    }
}