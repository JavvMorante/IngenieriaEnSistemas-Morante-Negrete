using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades_60MN;
using DAL_60MN;
using Servicios_60MN;

namespace BLL_60MN
{
    public class UsuarioBLL_60MN : AbstractBLL<Usuario_60MN>
    {
        FamiliaBLL_60MN _bllFamilias = new FamiliaBLL_60MN();

        public UsuarioBLL_60MN() 
        {
            _crud = new UsuarioDAL_60MN();
            SimularDatos();
        }

        private void SimularDatos()
        {
            _bllFamilias.SimularDatos();

            //u1 puede gestionar usuarios

            var u = new Usuario_60MN();
            u.Email = "u1@mail.com";
            u.Password = Encriptador_60MN.Hash("123");
            var f = _bllFamilias.GetAll().Where(ff => ff.Nombre.Contains("Gestores de usuarios")).FirstOrDefault();
            if (f != null) u.Permisos.Add(f);

            _crud.Save(u);

            //u2 puede gestionar permisos
            u = new Usuario_60MN();
            u.Email = "u2@mail.com";
            u.Password = Encriptador_60MN.Hash("123");
            f = _bllFamilias.GetAll().Where(ff => ff.Nombre.Contains("Gestores de permisos")).FirstOrDefault();
            if (f != null) u.Permisos.Add(f);
            _crud.Save(u);

            //admin tiene todo
            u = new Usuario_60MN();
            u.Email = "admin@mail.com";
            u.Password = Encriptador_60MN.Hash("123");
            f = _bllFamilias.GetAll().Where(ff => ff.Nombre.Contains("Administradores")).FirstOrDefault();
            if (f != null) u.Permisos.Add(f);

            _crud.Save(u);




        }


        public void Logout()
        {
            if (!SingletonSession_60MN.Instancia.IsLogged())
                throw new Exception("No hay sesión iniciada"); //doble validación, anulo en boton en formulario y valido en la bll


            SingletonSession_60MN.Instancia.Logout();
        }

    }
}
