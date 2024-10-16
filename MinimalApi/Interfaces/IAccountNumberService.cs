using MinimalApi.Models;
using MinimalApi.Repositories;

namespace MinimalApi.Interfaces
{
    public interface IAccountNumberService
    {
        public IEnumerable<AccountNumberModel> GetAccountNumbers();
        public AccountNumberModel GetAccountNumberByNumber(string number);
        public void AddAccountNumber(AccountNumberModel accountNumber);
        public bool DeleteAccountNumber(string id);
        public bool DeleteAll();
    }
}
