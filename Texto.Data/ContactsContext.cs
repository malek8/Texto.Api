using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Texto.Data
{
    public class ContactsContext : IContactsContext
    {
        private IMongoDatabase Database { get; }
        private static string CollectionName => "Contacts";

        public ContactsContext(IContextSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);

            Database = client.GetDatabase(settings.DatabaseName);
        }

        public T Get<T>(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var record = Database.GetCollection<BsonDocument>(CollectionName).Find(filter).FirstOrDefault();

            if (record != null)
            {
                return BsonSerializer.Deserialize<T>(record);
            }

            return (T) new object();
        }

        public IEnumerable<T> Get<T>(Func<T, bool> predicate)
        {
            return Database.GetCollection<T>(CollectionName).AsQueryable().Where(predicate);
        }

        public T Add<T>(T item)
        {
            Database.GetCollection<T>(CollectionName).InsertOne(item);
            return item;
        }

        public void Update<T>(string id, T item)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            Database.GetCollection<BsonDocument>(CollectionName).UpdateOne(filter, item.ToBsonDocument());
        }
    }
}
