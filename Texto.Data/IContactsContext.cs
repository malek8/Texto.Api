using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Texto.Data
{
    public interface IContactsContext
    {
        Task<T> Get<T>(string id);

        IEnumerable<T> Get<T>(Func<T, bool> predicate);

        Task<T> Add<T>(T item);

        Task Update<T>(string id, T item);
    }
}
