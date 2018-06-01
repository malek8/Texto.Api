using System.Linq;

namespace Texto.Data
{
    public interface IContactsContext
    {
        IQueryable Get<T>();

        T Add<T>(T item);
    }
}
