using Interfaces_60MN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades_60MN
{
    public class Entity_60MN : IEntity_60MN
    {
        public Entity_60MN() 
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
