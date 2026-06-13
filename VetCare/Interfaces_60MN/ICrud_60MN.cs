using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces_60MN
{
    public interface ICrud_60MN<T> where T : IEntity_60MN
    {
        T GetById(Guid id);
        IList<T> GetAll();
        void Save(T entity);

        void Delete(T entity);
    }
}
