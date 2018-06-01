using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Texto.Models
{
    public class Contact
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public ContactInfo Info { get; set; }

        [BsonElement]
        public Address Address { get; set; }

        [BsonElement]
        public IList<Message> Messages { get; set; }
    }

    public class ContactInfo
    {
        public string Number { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Message
    {
        public string Text { get; set; }
        public string Status { get; set; }
        public string Sid { get; set; }
        public long Order { get; set; }
        public MessageDirection Direction { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
