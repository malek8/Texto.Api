using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Contact> Get(string id)
        {
            return await Context.Get<Contact>(id);
        }

        public Contact GetByPhoneNumber(string phoneNumber)
        {
            return Context.Get<Contact>(x => x.Info.Number.Equals(phoneNumber, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public async Task<string> Add(Contact contact)
        {
            if (contact?.Info == null || string.IsNullOrEmpty(contact.Info.Number))
            {
                return string.Empty;
            }

            var existingContact = GetByPhoneNumber(contact.Info.Number);
            if (existingContact == null)
            {
                await Context.Add(contact);

                return contact.Id;
            }

            return existingContact.Id;
        }

        public async Task SendMessage(string contactId, Message message)
        {
            var contact = await Get(contactId);

            contact.Messages.Add(message);

            await Context.Update(contact.Id, contact);
        }
    }
}
