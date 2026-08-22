using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_60MN
{
    public abstract class Componente
    {
        string _nombre;

        public Componente(string nombre)
        {
            _nombre = nombre;

        }


        public abstract void AgregarHijo(Componente c);

        public abstract IList<Componente> obtenerhijos();


        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public abstract List<string> obtenerpatente { get; }



    }
}
