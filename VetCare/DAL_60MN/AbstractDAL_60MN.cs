using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces_60MN;
using Entidades_60MN;

namespace DAL_60MN
{
    public abstract class AbstractDAL_60MN<T> : ICrud_60MN<T> where T : IEntity_60MN
    {
        protected IList<T> dataContext;

        public AbstractDAL_60MN()
        {
            dataContext = new List<T>();
        }

        public void Delete(T entity)
        {
            this.dataContext.Remove(entity);
        }

        public IList<T> GetAll()
        {
            return dataContext;
        }

        public T GetById(Guid id)
        {
            return dataContext.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }

        public void Save(T entity)
        {
            if (dataContext.Contains(entity))
            {
                //si no fuesen objetos, habria que invocar la forma de actualizar el dato en el entorno de persistencia
            }
            else
            {
                dataContext.Add(entity);
            }
        }
    }
}
