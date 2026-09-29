using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_60MN.Seguridad_MN60
{
    public class LeafOperacion_60MN:Componente
    {
        private IList<Componente> _hijos = new List<Componente>();
        private List<string> listaoperaciones;
        ManejadorPerfilUsuarioBLL_60MN mpu = new ManejadorPerfilUsuarioBLL_60MN();

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


            throw new NotImplementedException();
        }



        public LeafOperacion_60MN(string nombre) : base(nombre)
        {


        }

        public IList<Componente> obtenerhijos(int usuid)
        {
            listaoperaciones = new List<string>();
            listaoperaciones = mpu.MostrarMenuPerfiles(usuid);


            foreach (var operacion in listaoperaciones)
            {
                foreach (var item in _hijos)
                {
                    LeafOperacion_60MN leaf = new LeafOperacion_60MN(operacion.ToString());

                    this.AgregarHijo(leaf);

                }

            }


            return _hijos;
        }
    }
}
