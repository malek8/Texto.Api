using System;
using System.Linq;
using Texto.Data;
using Texto.Models;

namespace Texto.Api.Services
{
    public class ContactsService : IContactsService
    {
        private IContactsContext Context { get; }

        public ContactsService(IContactsContext context)
        {
            this.Context = context;
        }

        public Contact Get(string id)
        {
            return Context.Get<Contact>(id);
        }

        public Contact GetByPhoneNumber(string phoneNumber)
        {
            return Context.Get<Contact>(x => x.Info.Number.Equals(phoneNumber, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public string Add(Contact contact)
        {
            if (contact?.Info == null || string.IsNullOrEmpty(contact.Info.Number))
            {
                return string.Empty;
            }

            var existingContact = GetByPhoneNumber(contact.Info.Number);
            if (existingContact == null)
            {
                Context.Add(contact);

                return contact.Id;
            }

            return existingContact.Id;
        }

        public void SendMessage(string contactId, Message message)
        {
            var contact = Get(contactId);

            contact.Messages.Add(message);

            Context.Update(contact.Id, contact);
        }
    }
}
