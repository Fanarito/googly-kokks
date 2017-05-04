using System.Collections.Generic;

namespace Kokks.Models
{
    public interface ISyntaxRepository
    {
        void Add(Syntax item);
        IEnumerable<Syntax> GetAll();
        Syntax Find(long id);
        void Remove(long id);
        void Update(Syntax item);
    }
}