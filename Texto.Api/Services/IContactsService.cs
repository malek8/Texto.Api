using System.Collections;
using System.Threading.Tasks;
using Texto.Models;

namespace Texto.Api.Services
{
    public interface IContactsService
    {
        Task<Contact> Get(string id);

        Task<Contact> GetByPhoneNumber(string phoneNumber);

        Task<string> Add(Contact contact);

        Task AddMessage(string contactId, Message message);

        Task UpdateAddress(string contactId, Address address);


    }
}
