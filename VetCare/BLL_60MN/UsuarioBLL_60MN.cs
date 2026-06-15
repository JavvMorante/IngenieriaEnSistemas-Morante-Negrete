using DAL_60MN;
using Entidades_60MN;
using Servicios_60MN;
using System.Data;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL_60MN
{
    public class UsuarioBLL_60MN : AbstractBLL<Usuario_60MN>
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

        public Usuario TraerDatosUsuariobyID(int usuid)
        {
            DAL.Usuario usu = new DAL.Usuario();
            Usuario usu1 = new Usuario();
            usu.TraerDatosUsuariobyID(usuid);
            //convierto un objeto dal a uno bll y traigo datos

            this.usuarioadapter(usu1, usu);

            return usu1;


        }

        public string traerDatosPerfil(string nombreUsuario)
        {
            string perfil;
            DAL.Usuario usu = new DAL.Usuario();
            perfil = usu.traerDatosPerfil(nombreUsuario);
            return perfil;

        }

        public int verificarDuplicidad(int dni, string email, string usuario)
        {
            int result = 0;
            DAL.Usuario usu = new DAL.Usuario();

            result = usu.verificarDuplicidad(dni, email, usuario);

            return result;

        }

        public List<string> MostraroperacionUsuario(string nombreUsuario)
        {
            List<string> listaopusuario = new List<string>();
            DAL.Usuario usu = new DAL.Usuario();

            listaopusuario = usu.MostraroperacionUsuario(nombreUsuario);

            return listaopusuario;
        }

        public List<string> MostrarOperacionesBloqueadas(string nombreUsuario)
        {
            List<string> listaopusuario = new List<string>();
            DAL.Usuario usu = new DAL.Usuario();

            listaopusuario = usu.MostrarOperacionesBloqueadas(nombreUsuario);

            return listaopusuario;



        }

        public static Usuario instancia;

        public static Usuario DevolverInstancia()
        {
            if (instancia == null)
            {
                instancia = new Usuario();
            }

            return instancia;
        }


        public void Dar_Alta_Usuario()
        {



            DAL.Usuario usu = new DAL.Usuario(_Usuario, Apellido, Nombre, Email, Dni, Habilitado, Clave);
            usu.Dar_Alta_Usuario(_Usuario, Apellido, Nombre, Email, Dni, Habilitado, Clave);
        }

        public string verificarPatentesBloqueo(string nombreUsuario, string patente)
        {
            DAL.ManejadorPerfilUsuario MPU = new DAL.ManejadorPerfilUsuario();
            string rta = MPU.verificarPatentesBloqueo(nombreUsuario, patente);

            return rta;



        }

        public Usuario TraerDatosUsuario(string usuario, string clave)
        {
            DAL.Usuario usu = new DAL.Usuario();
            Usuario usu1 = new Usuario();
            usu.TraerDatosUsuario(usuario, clave);
            //convierto un objeto dal a uno bll y traigo datos

            this.usuarioadapter(usu1, usu);

            return usu1;
        }

        public Usuario TraerDatosUsuario(string NombreUsuario)
        {
            Usuario BLL_USU = new BLL.Usuario();
            DAL.Usuario usu = new DAL.Usuario();
            usu = usu.TraerDatosUsuario(NombreUsuario);

            this.usuarioadapter(BLL_USU, usu);

            return BLL_USU;

        }

        public string ModificarDatosUsuario(string _Usuario, string apellido, string nombre, string email, Int64 dni, bool habilitado, int usuarioid)
        {


            DAL.Usuario usu = new DAL.Usuario();
            string flag = usu.ModificarDatosUsuario(_Usuario, apellido, nombre, email, dni, habilitado, usuarioid);


            return flag;

        }

        public string EliminarUsuario(int usuarioid)
        {
            DAL.Usuario usu = new DAL.Usuario();
            string result = usu.EliminarUsuario(usuarioid);

            return result;
        }

        public Usuario usuarioadapter(Usuario u, DAL.Usuario ud)
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
            DAL.Usuario usu = new DAL.Usuario();


            string rta = usu.CambiarClave(Usuario, ClaveNueva, Usuarioid);

            return rta;
        }

        public string verificarPatentesEscenciales(int usuarioID)
        {
            DAL.ManejadorPerfilUsuario MPU = new DAL.ManejadorPerfilUsuario();
            string rta = MPU.verificarPatentesEscenciales(usuarioID);

            return rta;
        }
        public string verificarPatentesEscenciales(string NombreUsuario)
        {
            DAL.ManejadorPerfilUsuario MPU = new DAL.ManejadorPerfilUsuario();
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
            DAL.Usuario USU = new DAL.Usuario();

            dt = USU.MostrarUsuarios();


            return dt;
        }

        public void Encriptar()
        {
            throw new System.NotImplementedException();
        }
        public string Clave { get; set; }

        //CONSTRUCTOR
        public Usuario(int Usuarioid, string _Usuario, string apellido, string _nombre, string _email, int _dni, bool _habilitado, int _FlagIntentos)
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
            DAL.Usuario USU = new DAL.Usuario();
            USU.SumarFlagIntentos(usuID);

        }

        public Usuario() { }



        public Usuario(string _Usuario, string apellido, string nombre, string email, int dni, bool habilitado, string clave)
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