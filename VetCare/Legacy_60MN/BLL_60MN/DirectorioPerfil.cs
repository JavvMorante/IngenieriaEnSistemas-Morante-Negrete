using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_60MN
{
    public class DirectorioPerfil: Componente
    {
        private List<string> listaoperaciones;
        ManejadorPerfilUsuarioBLL_60MN mpu = new ManejadorPerfilUsuarioBLL_60MN();


        private List<Componente> _hijos = new List<Componente>();
        public DirectorioPerfil(string nombre) : base(nombre)
        {
            //   _hijos = new List<Componente>();
        }

        public override List<string> obtenerpatente
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void AgregarHijo(Componente c)
        {
            _hijos.Add(c);
        }

        public override IList<Componente> obtenerhijos()
        {
            return _hijos.ToArray();
        }

        public IList<Componente> obtenerhijos(int usuid)
        {
            listaoperaciones = new List<string>();
            listaoperaciones = mpu.MostrarMenuPerfiles(usuid);

            foreach (var operacion in listaoperaciones)
            {
                Seguridad_MN60.LeafOperacion_60MN leaf = new Seguridad_MN60.LeafOperacion_60MN(operacion.ToString());
                _hijos.Add(leaf);

            }


            return _hijos;
        }
    }
}
