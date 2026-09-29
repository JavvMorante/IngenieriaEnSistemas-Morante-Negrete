using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_60MN
{
    public class ManejadorPerfilUsuarioBLL_60MN
    {
        private string _NombrePerfil;

        public string NombrePerfil
        {
            get { return _NombrePerfil; }
            set { _NombrePerfil = value; }
        }

        public string Operacionn
        { get; set; }

        public int PerfilID { get; set; }


        public List<string> DatasetOperaciones
        {
            get; set;
        }

        public int Usuario { get; set; }

        public void BloqueaOperacionUsuario(string NombreUsuario, string Patente)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();

            mpu.BloqueaOperacionUsuario(NombreUsuario, Patente);


        }

        /// <summary>
        ///Verifica en base si existe el perfil descrito
        /// </summary>
        public int VerificarAltafamilia(string nombrePerfil)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN MPU = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
            int rta = MPU.VerificarAltafamilia(nombrePerfil);

            return rta;
        }

        public DataTable BuscarPerfilUsuarios()
        {
            DataTable dt = new DataTable();
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN pu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();

            dt = pu.BuscarPerfilUsuarios();

            return dt;

        }


        public string _CrearPerfilUsuario(string NombrePerfil, string DescPerfil)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();

            string rta = mpu._CrearPerfilUsuario(NombrePerfil, DescPerfil);

            return rta;
        }

        public string ModificarPerfilUsuario(string NombrePerfil, string DescPerfil, int perfilID)
        {
            string rta = "False";

            try
            {
                DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();

                rta = mpu.ModificarPerfilUsuario(NombrePerfil, DescPerfil, perfilID);

                return rta;
            }
            catch (Exception ex)
            {

                return rta = ex.Message;
            }


        }






        public void DesbloqueaOperacionaUsuario(string NombreUsuario, String Patente)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();

            mpu.DesbloqueaOperacionaUsuario(NombreUsuario, Patente);




        }

        public string EliminarPerfilUsuario(int PerfilID)
        {
            string rta = "False";

            try
            {
                DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
                if (mpu.EliminarPerfilUsuario(PerfilID) == "True")
                {
                    rta = "True";
                }
                else
                {

                }

            }
            catch (Exception)
            {

                throw;
            }


            return rta;
        }

        public void GuardarUsuarios()
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Trae Descripcion de todas las operaciones del sistema
        /// </summary>
        public List<string> MostrarListaOperaciones()
        {
            List<string> listaoperaciones = new List<string>();
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();

            listaoperaciones = mpu.MostrarListaOperaciones();

            return listaoperaciones;
        }

        /// <summary>
        /// Trae Descripcion de todas las operaciones del sistema
        /// </summary>
        public List<string> MostrarListaOperaciones(int perfilID)
        {
            List<string> listaoperaciones = new List<string>();
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();

            listaoperaciones = mpu.MostrarListaOperaciones(perfilID);

            return listaoperaciones;
        }
        public List<string> MostrarListaOperaciones(string Perfil)
        {
            List<string> listaoperaciones = new List<string>();
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();

            listaoperaciones = mpu.MostrarListaOperaciones(Perfil);

            return listaoperaciones;
        }

        public void AsignarOperacionesalPerfil(int perfilID, List<string> listaoperacionesperfil)
        {

            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
            mpu.AsignarOperacionesalPerfil(perfilID, listaoperacionesperfil);



        }
        public void AsignarOperacionesalPerfil(string NombreUsuario, List<string> listaoperacionesperfil)
        {

            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
            mpu.AsignarOperacionesalPerfil(NombreUsuario, listaoperacionesperfil);



        }

        public List<string> MostrarMenuPerfiles(int Usuarioid)
        {
            List<string> listaoperaciones = new List<string>();
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN mpu = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();

            listaoperaciones = mpu.MostrarMenuperfiles(Usuarioid);



            return listaoperaciones;

        }

        public int AsignarUsuarioaPerfil(string nombreperfil, string nombreUsuario)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN MPU = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
            int FLAG;
            FLAG = MPU.AsignarUsuarioaPerfil(nombreperfil, nombreUsuario);

            return FLAG;
        }

        public void Validar()
        {
            throw new System.NotImplementedException();
        }

        public void VerificarOperacionesBloqueadas()
        {
            throw new System.NotImplementedException();
        }

        public void TraeOperaciones()
        {
            throw new System.NotImplementedException();
        }

        public void ConfirmarOperacion()
        {
            throw new System.NotImplementedException();
        }

        public void BuscarUsuarios()
        {
            throw new System.NotImplementedException();
        }

        public void ListarUsuarios()
        {
            throw new System.NotImplementedException();
        }

        public void Encriptar()
        {
            throw new System.NotImplementedException();
        }


    }
}
