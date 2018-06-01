using System.Collections;
using Texto.Models;

namespace Texto.Api.Services
{
    public interface IContactsService
    {
        Contact Get(string id);

        Contact GetByPhoneNumber(string phoneNumber);

        string Add(Contact contact);

        void SendMessage(string contactId, Message message);

    }
}
