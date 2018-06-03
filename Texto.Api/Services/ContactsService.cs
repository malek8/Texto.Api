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

        public async Task<Contact> GetByPhoneNumber(string phoneNumber)
        {
            return await Context.GetByPhoneNumber<Contact>(phoneNumber);
        }

        public async Task<string> Add(Contact contact)
        {
            if (contact?.Info == null || string.IsNullOrEmpty(contact.Info.Number))
            {
                return string.Empty;
            }

            var existingContact = await GetByPhoneNumber(contact.Info.Number);
            if (existingContact == null)
            {
                await Context.Add(contact);

                return contact.Id;
            }

            return existingContact.Id;
        }

        public async Task AddMessage(string contactId, Message message)
        {
            var contact = await Get(contactId);

            contact.Messages.Add(message);

            await Context.Update(contact.Id, contact);
        }

        public async Task UpdateAddress(string contactId, Address address)
        {
            var contact = await Get(contactId);

            contact.Address = address;

            await Context.Update(contact.Id, contact);
        }
    }
}
