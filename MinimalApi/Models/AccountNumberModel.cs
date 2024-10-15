using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MinimalApi.Models
{
    public class AccountNumberModel
    {
        public AccountNumberModel()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }
        public string Number { get; set; }
    }
}