using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces_60MN;
using Entidades_60MN;

namespace DAL_60MN
{
    public abstract class AbstractDAL_60MN<T> : ICrud_60MN<T>
         where T : IEntity_60MN
    {
        protected readonly Conexion_60MN conexion;

        protected AbstractDAL_60MN()
        {
            conexion = new Conexion_60MN();
        }

        public abstract void Save(T entity);

        public abstract void Delete(T entity);

        public abstract IList<T> GetAll();

        public abstract T GetById(long id);
        public abstract T GetById(Guid id);
    }
}

