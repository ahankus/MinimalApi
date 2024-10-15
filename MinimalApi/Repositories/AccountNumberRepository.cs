using MinimalApi.Interfaces;
using MinimalApi.Models;
using MongoDB.Driver;

namespace MinimalApi.Repositories
{
    public class AccountNumberRepository : IAccountNumberRepository
    {
        private readonly IMongoCollection<AccountNumberModel> _collection;

        public AccountNumberRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<AccountNumberModel>("AccountNumbers");
        }

        public AccountNumberModel GetByNumber(string number)
        {
            return _collection.Find(accountNumber => accountNumber.Number == number).FirstOrDefault();
        }

        public AccountNumberModel GetById(string id)
        {
            return _collection.Find(accountNumber => accountNumber.Id == id).FirstOrDefault();
        }

        public IEnumerable<AccountNumberModel> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        public void Add(AccountNumberModel accountNumber)
        {
            _collection.InsertOne(accountNumber);
        }

        public void Update(AccountNumberModel accountNumber)
        {
            _collection.ReplaceOne(an => an.Id == accountNumber.Id, accountNumber);
        }

        public void Delete(string id)
        {
            _collection.DeleteOne(accountNumber => accountNumber.Id == id);
        }

        public void DeleteAll()
        {
            _collection.DeleteMany(_ => true);
        }
    }
}