using DAL_60MN;
using Entidades_60MN;
using Servicios_60MN;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL_60MN
{
    public class UsuarioBLL_60MN : AbstractBLL<Usuario_60MN>
    {
        // Se eliminaron las variables privadas huérfanas como text1, text2, v, _habilitado, etc.

        // PROPIEDADES ACTUALIZADAS
        public int IdUsuario { get; set; }
        public int Dni { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int LoginCount { get; set; }
        public bool Locked { get; set; }
        public bool Deleted { get; set; }
        public string Clave { get; set; }

        public int DatasetOperaciones
        {
            get { throw new System.NotImplementedException(); }
            set { }
        }

        // MÉTODOS DE BÚSQUEDA Y MAPEO
        public UsuarioBLL_60MN TraerDatosUsuariobyID(int usuid)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            UsuarioBLL_60MN usu1 = new UsuarioBLL_60MN();

            // Guardamos el retorno de la DAL
            usu = usu.TraerDatosUsuariobyID(usuid);

            if (usu != null)
            {
                this.usuarioadapter(usu1, usu);
            }

            return usu1;
        }

        public string traerDatosPerfil(string nombreUsuario)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            return usu.traerDatosPerfil(nombreUsuario);
        }

        public int verificarDuplicidad(int dni, string email, string usuario)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            return usu.verificarDuplicidad(dni, email, usuario);
        }

        public List<string> MostraroperacionUsuario(string nombreUsuario)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            return usu.MostraroperacionUsuario(nombreUsuario);
        }

        public List<string> MostrarOperacionesBloqueadas(string nombreUsuario)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            return usu.MostrarOperacionesBloqueadas(nombreUsuario);
        }

        // SINGLETON
        public static UsuarioBLL_60MN instancia;

        public static UsuarioBLL_60MN DevolverInstancia()
        {
            if (instancia == null)
            {
                instancia = new UsuarioBLL_60MN();
            }
            return instancia;
        }

        // OPERACIONES DE ABM (ALTA, BAJA, MODIFICACIÓN)
        public void Dar_Alta_Usuario()
        {
            // Locked funciona al revés que habilitado: si está bloqueado, Locked es true.
            // Si la BLL no tiene la propiedad "Habilitado", usamos !Locked.
            bool isLocked = !Locked;

            // Instanciamos usando el constructor de 7 parámetros de la DAL
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN(Username, Apellido, Nombre, Email, Dni, isLocked, Clave);

            // Ejecutamos el método
            usu.Dar_Alta_Usuario(Username, Apellido, Nombre, Email, Dni, isLocked, Clave);
        }

        public string ModificarDatosUsuario(string usuario, string apellido, string nombre, string email, Int64 dni, bool habilitado, int usuarioid)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            return usu.ModificarDatosUsuario(usuario, apellido, nombre, email, dni, habilitado, usuarioid);
        }

        public string EliminarUsuario(int usuarioid)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            return usu.EliminarUsuario(usuarioid);
        }

        // CONSULTAS DE LOGIN
        public UsuarioBLL_60MN TraerDatosUsuario(string usuario, string clave)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            UsuarioBLL_60MN usu1 = new UsuarioBLL_60MN();

            // Capturamos la respuesta de la DAL (pasa por el método que devuelve UsuarioDAL_60MN o el adaptado)
            var datosDal = usu.TraerDatosUsuario(usuario);

            if (datosDal != null)
            {
                this.usuarioadapter(usu1, datosDal);
            }

            return usu1;
        }

        public UsuarioBLL_60MN TraerDatosUsuario(string NombreUsuario)
        {
            UsuarioBLL_60MN BLL_USU = new BLL_60MN.UsuarioBLL_60MN();
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            usu = usu.TraerDatosUsuario(NombreUsuario);

            if (usu != null)
            {
                this.usuarioadapter(BLL_USU, usu);
            }

            return BLL_USU;
        }

        // ADAPTADOR (MAPEO)
        public UsuarioBLL_60MN usuarioadapter(UsuarioBLL_60MN u, DAL_60MN.UsuarioDAL_60MN ud)
        {
            u.IdUsuario = ud.IdUsuario;
            u.Dni = ud.Dni;
            u.Apellido = ud.Apellido;
            u.Nombre = ud.Nombre;
            u.Rol = ud.Rol;
            u.Email = ud.Email;
            u.Username = ud.Username;
            u.Password = ud.Password;
            u.LoginCount = ud.LoginCount;
            u.Locked = ud.Locked;
            u.Deleted = ud.Deleted;

            return u;
        }

        // SEGURIDAD Y CLAVES
        public string CambiarClave(string Usuario, string ClaveNueva, int Usuarioid)
        {
            DAL_60MN.UsuarioDAL_60MN usu = new DAL_60MN.UsuarioDAL_60MN();
            return usu.CambiarClave(Usuario, ClaveNueva, Usuarioid);
        }

        public string verificarPatentesBloqueo(string nombreUsuario, string patente)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN MPU = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
            return MPU.verificarPatentesBloqueo(nombreUsuario, patente);
        }

        public string verificarPatentesEscenciales(int usuarioID)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN MPU = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
            return MPU.verificarPatentesEscenciales(usuarioID);
        }

        public string verificarPatentesEscenciales(string NombreUsuario)
        {
            DAL_60MN.ManejadorPerfilUsuarioDAL_60MN MPU = new DAL_60MN.ManejadorPerfilUsuarioDAL_60MN();
            return MPU.verificarPatentesEscenciales(NombreUsuario);
        }

        public void SumarFlagIntentos(int usuID)
        {
            DAL_60MN.UsuarioDAL_60MN USU = new DAL_60MN.UsuarioDAL_60MN();
            USU.SumarFlagIntentos(usuID);
        }

        public DataTable MostrarUsuarios()
        {
            DAL_60MN.UsuarioDAL_60MN USU = new DAL_60MN.UsuarioDAL_60MN();
            return USU.MostrarUsuarios();
        }

        // CONSTRUCTORES ACTUALIZADOS
        public UsuarioBLL_60MN() { }

        // Constructor Completo (10 parámetros principales)
        public UsuarioBLL_60MN(int idUsuario, string username, string apellido, string nombre, string rol, string email, int dni, bool locked, int loginCount, bool deleted)
        {
            this.IdUsuario = idUsuario;
            this.Username = username;
            this.Apellido = apellido;
            this.Nombre = nombre;
            this.Rol = rol;
            this.Email = email;
            this.Dni = dni;
            this.Locked = locked;
            this.LoginCount = loginCount;
            this.Deleted = deleted;
        }

        // Constructor para altas (7 parámetros)
        public UsuarioBLL_60MN(string username, string apellido, string nombre, string email, int dni, bool locked, string clave)
        {
            this.Username = username;
            this.Apellido = apellido;
            this.Nombre = nombre;
            this.Email = email;
            this.Dni = dni;
            this.Locked = locked;
            this.Clave = clave;
        }

        // MÉTODOS OVERRIDE O COMPLEMENTARIOS NO IMPLEMENTADOS
        public void VerificarOperacionesBloqueadas()
        {
            throw new System.NotImplementedException();
        }

        public void ValidarClaveNueva()
        {
            throw new System.NotImplementedException();
        }

        public void Encriptar()
        {
            throw new System.NotImplementedException();
        }

        public override Usuario_60MN GetById(long id)
        {
            throw new NotImplementedException();
        }
    }
}