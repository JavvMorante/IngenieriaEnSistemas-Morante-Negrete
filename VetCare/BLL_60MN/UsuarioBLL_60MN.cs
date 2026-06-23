using DAL_60MN;
using Entidades_60MN;
using Servicios_60MN;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL_60MN
{
    public class UsuarioBLL_60MN
    {
        private string text1;
        private string text2;
        private string text3;
        private string text4;
        private int v;
        private bool _habilitado;

        public int UsuarioID { get; set; }
        public string _Usuario { get; set; }

        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int Dni { get; set; }
        public bool Habilitado { get; set; }
        public int FlagIntentosLogin { get; set; }

        public int DatasetOperaciones
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {

            }
        }

        public UsuarioBLL_60MN TraerDatosUsuariobyID(int usuid)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            UsuarioBLL_60MN usu1 = new UsuarioBLL_60MN();
            usu.TraerDatosUsuariobyID(usuid);
            //convierto un objeto dal a uno bll y traigo datos

            this.usuarioadapter(usu1, usu);

            return usu1;


        }

        public string traerDatosPerfil(string nombreUsuario)
        {
            string perfil;
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            perfil = usu.traerDatosPerfil(nombreUsuario);
            return perfil;

        }

        public int verificarDuplicidad(int dni, string email, string usuario)
        {
            int result = 0;
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();

            result = usu.verificarDuplicidad(dni, email, usuario);

            return result;

        }

        public List<string> MostraroperacionUsuario(string nombreUsuario)
        {
            List<string> listaopusuario = new List<string>();
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();

            listaopusuario = usu.MostraroperacionUsuario(nombreUsuario);

            return listaopusuario;
        }

        public List<string> MostrarOperacionesBloqueadas(string nombreUsuario)
        {
            List<string> listaopusuario = new List<string>();
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();

            listaopusuario = usu.MostrarOperacionesBloqueadas(nombreUsuario);

            return listaopusuario;



        }

        public static UsuarioBLL_60MN instancia;

        public static UsuarioBLL_60MN DevolverInstancia()
        {
            if (instancia == null)
            {
                instancia = new UsuarioBLL_60MN();
            }

            return instancia;
        }


        public void Dar_Alta_Usuario()
        {



            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN(_Usuario, Apellido, Nombre, Email, Dni, Habilitado, Clave);
            usu.Dar_Alta_Usuario(_Usuario, Apellido, Nombre, Email, Dni, Habilitado, Clave);
        }

        public string verificarPatentesBloqueo(string nombreUsuario, string patente)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN MPU = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
            string rta = MPU.verificarPatentesBloqueo(nombreUsuario, patente);

            return rta;



        }

        public UsuarioBLL_60MN TraerDatosUsuario(string usuario, string clave)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            UsuarioBLL_60MN usu1 = new UsuarioBLL_60MN();
            usu.TraerDatosUsuario(usuario, clave);
            //convierto un objeto dal a uno bll y traigo datos

            this.usuarioadapter(usu1, usu);

            return usu1;
        }

        public UsuarioBLL_60MN TraerDatosUsuario(string NombreUsuario)
        {
            UsuarioBLL_60MN BLL_USU = new BLL_60MN.UsuarioBLL_60MN();
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            usu = usu.TraerDatosUsuario(NombreUsuario);

            this.usuarioadapter(BLL_USU, usu);

            return BLL_USU;

        }

        public string ModificarDatosUsuario(string _Usuario, string apellido, string nombre, string email, Int64 dni, bool habilitado, int usuarioid)
        {


            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            string flag = usu.ModificarDatosUsuario(_Usuario, apellido, nombre, email, dni, habilitado, usuarioid);


            return flag;

        }

        public string EliminarUsuario(int usuarioid)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            string result = usu.EliminarUsuario(usuarioid);

            return result;
        }

        public UsuarioBLL_60MN usuarioadapter(UsuarioBLL_60MN u, DAL_60MN.UsuarioDAL_60MN ud)
        {

            u.UsuarioID = ud.UsuarioID;
            u.Apellido = ud.apellido;
            u.Nombre = ud.nombre;
            u.Email = ud.email;
            u.Dni = ud.dni;
            u.Habilitado = ud.habilitado;
            u._Usuario = ud._Usuario;
            u.FlagIntentosLogin = ud.FlagIntentosLogin;

            return u;
        }

        public void VerificarOperacionesBloqueadas()
        {
            throw new System.NotImplementedException();
        }

        public string CambiarClave(string Usuario, string ClaveNueva, int Usuarioid)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();


            string rta = usu.CambiarClave(Usuario, ClaveNueva, Usuarioid);

            return rta;
        }

        public string verificarPatentesEscenciales(int usuarioID)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN MPU = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
            string rta = MPU.verificarPatentesEscenciales(usuarioID);

            return rta;
        }
        public string verificarPatentesEscenciales(string NombreUsuario)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN MPU = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
            string rta = MPU.verificarPatentesEscenciales(NombreUsuario);

            return rta;
        }

        public void ValidarClaveNueva()
        {
            throw new System.NotImplementedException();
        }

        public DataTable MostrarUsuarios()
        {
            DataTable dt = new DataTable();
            DAL_60MN.UsuarioDAL_60MN USU = new DAL_60MN.UsuarioDAL_60MN();

            dt = USU.MostrarUsuarios();


            return dt;
        }

        public void Encriptar()
        {
            throw new System.NotImplementedException();
        }
        public string Clave { get; set; }

        //CONSTRUCTOR
        public UsuarioBLL_60MN(int Usuarioid, string _Usuario, string apellido, string _nombre, string _email, int _dni, bool _habilitado, int _FlagIntentos)
        {
            this.UsuarioID = Usuarioid;
            this._Usuario = _Usuario;
            this.Apellido = apellido;
            this.Nombre = _nombre;
            this.Email = _email;
            this.Dni = _dni;
            this.Habilitado = _habilitado;
            this.FlagIntentosLogin = _FlagIntentos;

        }

        public void SumarFlagIntentos(int usuID)
        {
            DAL_60MN.UsuarioDAL_60MN USU = new DAL_60MN.UsuarioDAL_60MN();
            USU.SumarFlagIntentos(usuID);

        }

        public UsuarioBLL_60MN() { }



        public UsuarioBLL_60MN(string _Usuario, string apellido, string nombre, string email, int dni, bool habilitado, string clave)
        {
            this._Usuario = _Usuario;
            this.Apellido = apellido;
            this.Nombre = nombre;
            this.Email = email;
            this.Dni = dni;
            this.Habilitado = habilitado;
            this.Clave = clave;
        }



    }
}