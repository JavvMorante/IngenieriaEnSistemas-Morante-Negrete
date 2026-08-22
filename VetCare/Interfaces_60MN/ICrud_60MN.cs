using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces_60MN
{
    public interface ICrud_60MN<T> where T : IEntity_60MN
    {
        void Save(T entity);

        void Delete(T entity);

        IList<T> GetAll();

        T GetById(long id);
        T GetById(Guid id);
    }
}
