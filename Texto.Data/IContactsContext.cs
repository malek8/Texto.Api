using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Texto.Data
{
    public interface IContactsContext
    {
        T Get<T>(string id);

        IEnumerable<T> Get<T>(Func<T, bool> predicate);

        T Add<T>(T item);

        void Update<T>(string id, T item);
    }
}
