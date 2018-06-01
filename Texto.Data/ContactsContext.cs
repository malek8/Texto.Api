using System.Linq;
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

        public IQueryable Get<T>()
        {
            return Database.GetCollection<T>(CollectionName).AsQueryable();
        }

        public T Add<T>(T item)
        {
            Database.GetCollection<T>(CollectionName).InsertOne(item);
            return item;
        }
    }
}
