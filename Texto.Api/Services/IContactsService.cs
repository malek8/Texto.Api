using System.Collections;
using System.Threading.Tasks;
using Texto.Models;

namespace Texto.Api.Services
{
    public interface IContactsService
    {
        Task<Contact> Get(string id);

        Contact GetByPhoneNumber(string phoneNumber);

        Task<string> Add(Contact contact);

        Task SendMessage(string contactId, Message message);

    }
}
