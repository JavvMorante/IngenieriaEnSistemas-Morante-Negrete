using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces_60MN;
using Entidades_60MN;

namespace BLL_60MN
{
    public abstract class AbstractBLL<T> : ICrud_60MN<T> where T : IEntity_60MN
    {
        protected ICrud_60MN<T> _crud;

        public void Delete(T entity)
        {
            _crud.Delete(entity);
        }

        public IList<T> GetAll()
        {
            return _crud.GetAll();

        }

        public T GetById(Guid id)
        {
            return _crud.GetById(id);
        }

        public void Save(T entity)
        {
            _crud.Save(entity);
        }
    }
}