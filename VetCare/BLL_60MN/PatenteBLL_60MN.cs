using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades_60MN;
using BLL_60MN;
using DAL_60MN;
using Interfaces_60MN;
using Servicios_60MN.Composite;

namespace BLL_60MN
{
    public class PatenteBLL_60MN: AbstractBLL<IPatente_60MN>
    {
        public PatenteBLL_60MN()
        {
            _crud = new PatenteDAL_60MN();

        }


        public void SimularDatos()
        {
            var p = new Patente_60MN();
            p.Nombre = "Puede gestionar usuarios";
            p.Tipo = TipoPermiso.GestorUsuario;
            _crud.Save(p);

            p = new Patente_60MN();
            p.Nombre = "Puede gestionar permisos";
            p.Tipo = TipoPermiso.GestorPermiso;
            _crud.Save(p);
        }
    }
}
