using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public async Task<T> Get<T>(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));

            try
            {
                var record = await Database.GetCollection<BsonDocument>(CollectionName).FindAsync(filter);

                if (record != null)
                {
                    return BsonSerializer.Deserialize<T>(record.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {

            }

            return default(T);
        }

        public IEnumerable<T> Get<T>(Expression<Func<T, bool>> predicate)
        {
            return Database.GetCollection<T>(CollectionName).AsQueryable().Where(predicate);
        }

        public async Task<T> GetByPhoneNumber<T>(string number)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Info.Number", number);
            var record = await Database.GetCollection<BsonDocument>(CollectionName).FindAsync(filter);

            if (record != null)
            {
                return BsonSerializer.Deserialize<T>(record.FirstOrDefault());
            }

            return default(T);
        }

        public async Task<T> Add<T>(T item)
        {
            try
            {
                await Database.GetCollection<T>(CollectionName).InsertOneAsync(item);
            }
            catch (Exception ex)
            {

            }

            return item;
        }

        public async Task Update<T>(string id, T item)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            var document = new BsonDocument("$set", item.ToBsonDocument());

            try
            {
                await Database.GetCollection<BsonDocument>(CollectionName).UpdateOneAsync(filter, document);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
