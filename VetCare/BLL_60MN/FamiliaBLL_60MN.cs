using Interfaces_60MN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL_60MN;
using DAL_60MN;
//using Servicios_60MN.Composite;

namespace BLL_60MN
{
    public class FamiliaBLL_60MN : AbstractBLL<IFamilia_60MN>
    {
       PatenteBLL_60MN _bllPatentes = new PatenteBLL_60MN();

        public FamiliaBLL_60MN()
        {
            _crud = new FamiliaDAL_60MN();
        }

        public override IFamilia_60MN GetById(long id)
        {
            throw new NotImplementedException();
        }

      /*  public void SimularDatos()
        {
            _bllPatentes.SimularDatos();

            var f1 = new Familia_60MN();
            f1.Nombre = "Gestores de usuarios";
            var p1 = _bllPatentes.GetAll().Where(pp => pp.Nombre.Contains("Puede gestionar usuarios")).FirstOrDefault();
            if (p1 != null) f1.AgregarPermiso(p1);
            _crud.Save(f1);

            var f2 = new Familia_60MN();
            var p2 = _bllPatentes.GetAll().Where(pp => pp.Nombre.Contains("Puede gestionar permisos")).FirstOrDefault();
            if (p2 != null) f2.AgregarPermiso(p2);

            f2.Nombre = "Gestores de permisos";
            _crud.Save(f2);

            var f3 = new Familia_60MN();
            f3.Nombre = "Administradores";
            f3.AgregarPermiso(f1);
            f3.AgregarPermiso(f2);
            _crud.Save(f3);
        }*/
        
    }
}
