using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MinimalApi.Models
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }
        public string Name { get; set; }
        public AccountNumberModel AccountNumber { get; set; }
    }
}