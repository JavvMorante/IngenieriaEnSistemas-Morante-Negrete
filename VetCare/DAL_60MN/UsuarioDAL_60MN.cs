using Entidades_60MN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;

namespace DAL_60MN
{
    [Serializable]
    public class UsuarioDAL_60MN : AbstractDAL_60MN<Usuario_60MN>
    {

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

        Conexion_60MN con = new Conexion_60MN();
        DigitosVerificadores_60MN dv = new DigitosVerificadores_60MN();
        DataTable dt = new DataTable();

        // CONSTRUCTOR ACTUALIZADO
        // Modificamos los parámetros para recibir el nuevo modelo de datos

        public UsuarioDAL_60MN() { }
        public UsuarioDAL_60MN(int idUsuario, string username, string apellido, string nombre, string rol, string email, int dni, bool locked, int loginCount, bool deleted)
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

        public UsuarioDAL_60MN TraerDatosUsuariobyID(int usuid)
        {
            // 1. Apuntamos a la tabla real y seleccionamos las columnas reales en orden exacto
            string sql = "SELECT IdUsuario, username, password, nombre, apellido, dni, email, rol, locked, logincount, deleted " +
                         "FROM [VetCareBD].[dbo].[Usuario_60MN] " +
                         "WHERE IdUsuario = " + usuid;

            try
            {
                DataTable dt = con.Ejecutarreader(sql);

                // Si no encuentra filas, devolvemos null de forma segura
                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }

                DataRow row = dt.Rows[0];

                // 2. Mapeamos los datos de las columnas a las propiedades de ESTA instancia (this)
                // Usamos validación de nulos por seguridad
                this.IdUsuario = row["IdUsuario"] != DBNull.Value ? Convert.ToInt32(row["IdUsuario"]) : 0;
                this.Username = row["username"] != DBNull.Value ? row["username"].ToString() : string.Empty;
                this.Password = row["password"] != DBNull.Value ? row["password"].ToString() : string.Empty;
                this.Nombre = row["nombre"] != DBNull.Value ? row["nombre"].ToString() : string.Empty;
                this.Apellido = row["apellido"] != DBNull.Value ? row["apellido"].ToString() : string.Empty;
                this.Dni = row["dni"] != DBNull.Value ? Convert.ToInt32(row["dni"]) : 0;
                this.Email = row["email"] != DBNull.Value ? row["email"].ToString() : string.Empty;
                this.Rol = row["rol"] != DBNull.Value ? row["rol"].ToString() : string.Empty;
                this.Locked = row["locked"] != DBNull.Value ? Convert.ToBoolean(row["locked"]) : false;
                this.LoginCount = row["logincount"] != DBNull.Value ? Convert.ToInt32(row["logincount"]) : 0;
                this.Deleted = row["deleted"] != DBNull.Value ? Convert.ToBoolean(row["deleted"]) : false;

                // Imprimimos en consola usando la nueva propiedad para verificar
                Console.WriteLine("Entró reader ID: " + this.IdUsuario);

                // 3. Instanciamos el objeto usando el NUEVO constructor que armamos antes
                UsuarioDAL_60MN usu = new UsuarioDAL_60MN(
                    this.IdUsuario,
                    this.Username,
                    this.Apellido,
                    this.Nombre,
                    this.Rol,
                    this.Email,
                    this.Dni,
                    this.Locked,
                    this.LoginCount,
                    this.Deleted
                );

                // Agregamos el mapeo de la contraseña por si la necesitás fuera del constructor
                usu.Password = this.Password;

                return usu;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string traerDatosPerfil(string nombreUsuario)
        {

            string rta = "";

            string sql = "select p.NombrePerfil from perfilusuario p " +
" inner join UsuarioFamilia uf on uf.PerfilID = p.PerfilUsuarioID " +
" inner join Usuario u on u.UsuarioID = uf.UsuarioID " +
" where u.Usuario like '%" + nombreUsuario + "%'";

            dt = con.Ejecutarreader(sql);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    rta = item[0].ToString();
                }
            }


            return rta;


        }

        public List<string> MostrarOperacionesBloqueadas(string nombreUsuario)
        {

            List<string> listaoperaciones1 = new List<string>();
            listaoperaciones1.Clear();
            DataTable dt = new DataTable();

            string sql = "Select op.Descripcion from usuariooperacion uo inner join " +
                       " operacion op on op.OperacionID = uo.Operacionid " +
                       " where uo.UsuarioID = (select UsuarioID from Usuario where" +
                       " Usuario like '%" + nombreUsuario + "%')" +
                       " and Habilitado like 'N';";


            dt = con.Ejecutarreader(sql);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                { listaoperaciones1.Add(dt.Rows[i]["Descripcion"].ToString()); }

            }

            return listaoperaciones1;



        }

        public List<string> MostraroperacionUsuario(string nombreUsuario)
        {

            List<string> listaoperaciones1 = new List<string>();
            listaoperaciones1.Clear();
            DataTable dt = new DataTable();

            string sql = "Select op.Descripcion from usuariooperacion uo inner join " +
                       " operacion op on op.OperacionID = uo.Operacionid " +
                       " where uo.UsuarioID = (select UsuarioID from Usuario where" +
                       " Usuario like '%" + nombreUsuario + "%');";

            dt = con.Ejecutarreader(sql);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                { listaoperaciones1.Add(dt.Rows[i]["Descripcion"].ToString()); }

            }

            return listaoperaciones1;




        }

        public int verificarDuplicidad(int dni, string email, string usuario)
        {
            int result;
            string sql = " DECLARE @Dni int = (select count(dni) from usuario where dni = " + dni + ")" +
             "  DECLARE @Email varchar(50) = (select count(Email) from Usuario where Email like '" + email + "')" +
             " DECLARE @Usuario varchar(50) = (select count(Usuario) from Usuario where Usuario like '" + usuario + "')" +
             " declare @result int = 4;" +
           " set @result = (select CASE when @Email = 1 then 2 ELSE @result END )" +
           "   set @result = (select CASE when @Dni = 1 then  1 ELSE @result END )" +
           "	  set @result = (select CASE when @Usuario = 1 then 5 ELSE @result end) " +
           "   set @result = (select CASE when @Dni = 1 AND @Email = 1 then 3 ELSE @result end) " +
           "  select @result;";





            DataTable dt = new DataTable();
            dt = con.Ejecutarreader(sql);

            if (dt.Rows.Count > 0)
            {
                Console.WriteLine("entró reader " + Convert.ToString(dt.Rows[0][0].ToString()));
                result = Convert.ToInt32(dt.Rows[0][0].ToString());
                return result;
            }
            else
            {
                return result = 5;
            }


        }

        public string ModificarDatosUsuario(string _Usuario, string apellido, string nombre, string email, Int64 dni, bool habilitado, int usuarioid)
        {

            int h;
            if (habilitado.ToString() == "True")
            {
                h = 1;
            }
            else
            {
                h = 0;
            }
            string sql = "update Usuario set Usuario = '" + _Usuario + "',apellido = '" + apellido + "',nombre='" + nombre + "',DNI=" + dni + ",email='" + email + "',Habilitado=" + h + " "
            + "where Usuarioid = " + usuarioid + ";";

            string result = con.Ejecutar(sql);

            dv.RecalcularDVH();
            return result;

        }

        public string EliminarUsuario(int usuarioid)
        {
            string sql = " Delete UsuarioOperacion where Usuarioid =" + usuarioid + " " +
                         " Delete usuariofamilia where Usuarioid =" + usuarioid + " " +
                         " Delete usuario where usuarioid = " + usuarioid + ";";

            string result = con.Ejecutar(sql);

            dv.RecalcularDVH();
            return result;
        }

        // 1. SEGUNDO CONSTRUCTOR (El que no lleva ID, ideal para altas de usuario)
        public UsuarioDAL_60MN(string username, string apellido, string nombre, string email, int dni, bool locked, string password)
        {
            this.Username = username;
            this.Apellido = apellido;
            this.Nombre = nombre;
            this.Email = email;
            this.Dni = dni;
            this.Locked = locked;
            this.Password = password;
        }

        // 2. DAR DE ALTA USUARIO
        public void Dar_Alta_Usuario(string username, string apellido, string nombre, string email, int dni, bool locked, string password)
        {
            // Ajustamos las columnas reales según tu tabla. 
            // Locked = 0 significa activo. Deleted = 0 significa no borrado.
            int lockedValue = locked ? 1 : 0;

            string sql = $"INSERT INTO [VetCareBD].[dbo].[Usuario_60MN] " +
                         $"(dni, apellido, nombre, rol, email, username, password, logincount, locked, deleted) " +
                         $"VALUES ({dni}, '{apellido}', '{nombre}', 'Usuario', '{email}', '{username}', '{password}', 0, {lockedValue}, 0)";

            string result = con.Ejecutar(sql);
            dv.RecalcularDVH();
        }

        // 3. TRAER DATOS USUARIO (CON LOGUEO - Mapeo a entidad Usuario_60MN)
        public Usuario_60MN TraerDatosUsuario(string usuario, string clave)
        {
            string sql = "SELECT IdUsuario, dni, apellido, nombre, rol, email, username, password, logincount, locked, deleted " +
                         "FROM [VetCareBD].[dbo].[Usuario_60MN] " +
                         "WHERE username = '" + usuario + "' AND password = '" + clave + "' AND locked = 0 AND deleted = 0";

            try
            {
                DataTable dt = con.Ejecutarreader(sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }

                DataRow row = dt.Rows[0];

                Usuario_60MN usu = new Usuario_60MN
                {
                    IdUsuario = row["IdUsuario"] != DBNull.Value ? Convert.ToInt32(row["IdUsuario"]) : 0,
                    Dni = row["dni"] != DBNull.Value ? Convert.ToInt32(row["dni"]) : 0,
                    Apellido = row["apellido"] != DBNull.Value ? row["apellido"].ToString() : string.Empty,
                    Nombre = row["nombre"] != DBNull.Value ? row["nombre"].ToString() : string.Empty,
                    Rol = row["rol"] != DBNull.Value ? row["rol"].ToString() : string.Empty,
                    Email = row["email"] != DBNull.Value ? row["email"].ToString() : string.Empty,
                    Username = row["username"] != DBNull.Value ? row["username"].ToString() : string.Empty,

                    // OJO ACÁ: Cambié PasswordHash por Password para que coincida con tu entidad renovada
                    PasswordHash = row["password"] != DBNull.Value ? row["password"].ToString() : string.Empty,

                    LoginCount = row["logincount"] != DBNull.Value ? Convert.ToInt32(row["logincount"]) : 0,
                    Locked = row["locked"] != DBNull.Value ? Convert.ToBoolean(row["locked"]) : false,
                    Deleted = row["deleted"] != DBNull.Value ? Convert.ToBoolean(row["deleted"]) : false
                };

                return usu;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // 4. MOSTRAR USUARIOS (Para grillas/tablas)
        public DataTable MostrarUsuarios()
        {
            // Cambiamos los nombres viejos de las columnas por los reales de la base de datos actual
            string sql = "SELECT IdUsuario, username, password, nombre, apellido, dni, email, rol, locked, logincount FROM [VetCareBD].[dbo].[Usuario_60MN] WHERE deleted = 0";

            dt = con.Ejecutarreader(sql);
            return dt;
        }

        // 5. TRAER DATOS USUARIO (Por nombre de usuario - Mapeo a UsuarioDAL_60MN)
        public UsuarioDAL_60MN TraerDatosUsuario(string usuario)
        {
            string sql = "SELECT IdUsuario, username, password, nombre, apellido, dni, email, rol, locked, logincount, deleted " +
                         "FROM [VetCareBD].[dbo].[Usuario_60MN] " +
                         "WHERE username = '" + usuario + "'";

            try
            {
                dt = con.Ejecutarreader(sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }

                DataRow row = dt.Rows[0];

                // Mapeamos a las variables globales de "this" usando los nombres nuevos
                this.IdUsuario = row["IdUsuario"] != DBNull.Value ? Convert.ToInt32(row["IdUsuario"]) : 0;
                this.Username = row["username"] != DBNull.Value ? row["username"].ToString() : string.Empty;
                this.Password = row["password"] != DBNull.Value ? row["password"].ToString() : string.Empty;
                this.Nombre = row["nombre"] != DBNull.Value ? row["nombre"].ToString() : string.Empty;
                this.Apellido = row["apellido"] != DBNull.Value ? row["apellido"].ToString() : string.Empty;
                this.Dni = row["dni"] != DBNull.Value ? Convert.ToInt32(row["dni"]) : 0;
                this.Email = row["email"] != DBNull.Value ? row["email"].ToString() : string.Empty;
                this.Rol = row["rol"] != DBNull.Value ? row["rol"].ToString() : string.Empty;
                this.Locked = row["locked"] != DBNull.Value ? Convert.ToBoolean(row["locked"]) : false;
                this.LoginCount = row["logincount"] != DBNull.Value ? Convert.ToInt32(row["logincount"]) : 0;
                this.Deleted = row["deleted"] != DBNull.Value ? Convert.ToBoolean(row["deleted"]) : false;

                Console.WriteLine("Entró reader Username: " + this.Username);

                // Usamos el constructor principal (el de 10 parámetros) para armar el retorno
                UsuarioDAL_60MN usu = new UsuarioDAL_60MN(
                    this.IdUsuario,
                    this.Username,
                    this.Apellido,
                    this.Nombre,
                    this.Rol,
                    this.Email,
                    this.Dni,
                    this.Locked,
                    this.LoginCount,
                    this.Deleted
                );

                usu.Password = this.Password;

                return usu;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string CambiarClave(string usu, string claveNueva, int Usuarioid)
        {
            string sql = "Update Usuario set Clave = '" + claveNueva + "' " +
                 " where Usuario = '" + usu + "'" +
                 " and Usuarioid = " + Usuarioid + "";

            string rta = con.Ejecutar(sql);
            dv.RecalcularDVH();
            return rta;




        }

        public void SumarFlagIntentos(int usuID)
        {
            string sp = "SumarFlagIntentos";
            con.EjecutarProcedure(sp, usuID);
        }

        public override void Save(Usuario_60MN entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Usuario_60MN entity)
        {
            throw new NotImplementedException();
        }

        public override IList<Usuario_60MN> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Usuario_60MN GetById(long id)
        {
            throw new NotImplementedException();
        }

        public override Usuario_60MN GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}