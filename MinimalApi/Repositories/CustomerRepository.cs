using MinimalApi.Interfaces;
using MinimalApi.Models;
using MongoDB.Driver;

namespace MinimalApi.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<CustomerModel> _collection;

        public CustomerRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<CustomerModel>("Customers");
        }

        public CustomerModel GetById(string id)
        {
            // Query MongoDB for the customer by ID
            return _collection.Find(c => c.Id == id).FirstOrDefault();
        }

        public IEnumerable<CustomerModel> GetAll()
        {
            // Retrieve all customers from MongoDB
            return _collection.Find(_ => true).ToList();
        }

        public void Add(CustomerModel customer)
        {
            // Insert the customer into MongoDB
            _collection.InsertOne(customer);
        }

        public void Update(CustomerModel customer)
        {
            // Update the customer in MongoDB
            _collection.ReplaceOne(c => c.Id == customer.Id, customer);
        }

        public void Delete(string id)
        {
            // Delete the customer from MongoDB
            _collection.DeleteOne(c => c.Id == id);
        }

        public void DeleteAll()
        {
            _collection.DeleteMany(_ => true);
        }
    }
}