using MinimalApi.Models;

namespace MinimalApi.Interfaces
{
    public interface IAccountNumberRepository
    {
        AccountNumberModel GetById(string id);
        AccountNumberModel GetByNumber(string number);
        IEnumerable<AccountNumberModel> GetAll();
        void Add(AccountNumberModel accountNumber);
        void Update(AccountNumberModel accountNumber);
        void Delete(string id);
        void DeleteAll();
    }
}