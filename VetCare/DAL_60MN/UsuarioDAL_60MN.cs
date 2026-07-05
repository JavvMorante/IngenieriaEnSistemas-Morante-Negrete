using Entidades_60MN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;

namespace DAL_60MN
{
    [Serializable]
    public class UsuarioDAL_60MN
    {
        public string apellido { get; set; }
        public int dni { get; set; }
        public string email { get; set; }
        public bool habilitado { get; set; }
        public string nombre { get; set; }
        public string _Usuario { get; set; }
        public int FlagIntentosLogin { get; set; }
        public int UsuarioID { get; set; }


        Conexion_60MN con = new Conexion_60MN();
        DigitosVerificadores_60MN dv = new DigitosVerificadores_60MN();

        DataTable dt = new DataTable();

        public string Clave { get; set; }
        public UsuarioDAL_60MN(int Usuarioid, string _Usuario, string apellido, string nombre, string email, int dni, bool habilitado, int FlagIntentos)
        {
            this.UsuarioID = Usuarioid;
            this._Usuario = _Usuario;
            this.apellido = apellido;
            this.nombre = nombre;
            this.email = email;
            this.dni = dni;
            this.habilitado = habilitado;
            this.FlagIntentosLogin = FlagIntentos;

        }

        public UsuarioDAL_60MN TraerDatosUsuariobyID(int usuid)
        {

            string sql = "select UsuarioID,Usuario,Clave,Nombre,Apellido,DNI,Email,Habilitado,FlagIntentosLogin From Usuario " +
                          "where usuarioid = " + usuid + "";


            try
            {
                DataTable dt = new DataTable();
                dt = con.Ejecutarreader(sql);



                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine("entró reader " + Convert.ToString(dt.Rows[0][0].ToString()));

                    if (dt.Rows.Count > 0)
                    {
                        // Usamos el nombre de la columna directamente en lugar de números
                        this.UsuarioID = Convert.ToInt32(dt.Rows[0]["UsuarioID"]);
                        this._Usuario = Convert.ToString(dt.Rows[0]["Usuario"]);
                        this.nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
                        this.apellido = Convert.ToString(dt.Rows[0]["Apellido"]);
                        this.dni = Convert.ToInt32(dt.Rows[0]["DNI"]); 
                        this.email = Convert.ToString(dt.Rows[0]["Email"]);
                        //this.habilitado = Convert.ToBoolean(dt.Rows[0]["Habilitado"]);
                        // Si es nulo en la BD, le asignamos false por defecto. Si no, lo convertimos de forma segura.
                        this.habilitado = dt.Rows[0]["Habilitado"] != DBNull.Value && Convert.ToBoolean(dt.Rows[0]["Habilitado"]);
                        this.FlagIntentosLogin = Convert.ToInt32(dt.Rows[0]["FlagIntentosLogin"]);
                    }

                }
                else
                {


                }

                UsuarioDAL_60MN usu = new UsuarioDAL_60MN(this.UsuarioID, this._Usuario, this.apellido, this.nombre, this.email, this.dni, this.habilitado, this.FlagIntentosLogin);

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

        public string ModificarDatosUsuario(int usuarioid, string _Usuario = null, string apellido = null, string nombre = null, string email = null, Int64? dni = null, bool? habilitado = null)
        {

            /*int h;
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
            return result;*/

            // List para ir guardando los fragmentos de las columnas a actualizar
            List<string> columnasAActualizar = new List<string>();

            // Vamos evaluando uno por uno si el parámetro fue enviado (no es nulo)
            if (!string.IsNullOrEmpty(_Usuario))
            {
                columnasAActualizar.Add("Usuario = '" + _Usuario + "'");
            }

            if (!string.IsNullOrEmpty(apellido))
            {
                columnasAActualizar.Add("apellido = '" + apellido + "'");
            }

            if (!string.IsNullOrEmpty(nombre))
            {
                columnasAActualizar.Add("nombre = '" + nombre + "'");
            }

            if (!string.IsNullOrEmpty(email))
            {
                columnasAActualizar.Add("email = '" + email + "'");
            }

            if (dni.HasValue)
            {
                columnasAActualizar.Add("DNI = " + dni.Value);
            }

            if (habilitado.HasValue)
            {
                int h = habilitado.Value ? 1 : 0;
                columnasAActualizar.Add("Habilitado = " + h);
            }

            // Si no se pasó ningún parámetro para modificar, salimos sin hacer nada
            if (columnasAActualizar.Count == 0)
            {
                return "No se especificaron cambios para actualizar.";
            }

            // Unimos todos los fragmentos usando una coma como separador
            string setSql = string.Join(", ", columnasAActualizar);

            // Armamos el query final
            string sql = "UPDATE Usuario SET " + setSql + " WHERE Usuarioid = " + usuarioid + ";";

            // Ejecutamos en la base de datos
            string result = con.Ejecutar(sql);

            // Como bien tenías, recalculamos los dígitos verificadores por consistencia
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

        public UsuarioDAL_60MN() { }

        public UsuarioDAL_60MN(string _Usuario, string apellido, string nombre, string email, int dni, bool habilitado, string clave)
        {
            this._Usuario = _Usuario;
            this.apellido = apellido;
            this.nombre = nombre;
            this.email = email;
            this.dni = dni;
            this.habilitado = habilitado;
            this.Clave = clave;
        }

        public void Dar_Alta_Usuario(string _Usuario, string apellido, string nombre, string email, int dni, bool habilitado, string clave)
        {


            string sql = "insert into Usuario values( '" + _Usuario + "',"
               + "'" + clave + "','" + nombre + "','" + apellido + "'," + dni + ",'" + email + "','" + habilitado + "',0," + "'_DVH')";

            string result = con.Ejecutar(sql);
            dv.RecalcularDVH();
        }

        public UsuarioDAL_60MN TraerDatosUsuario(string usuario, string clave)
        {


            string sql = "select UsuarioID,Usuario,Clave,Nombre,Apellido,DNI,Email,Habilitado,FlagIntentosLogin From Usuario " +
                          " where usuario = '" + usuario + "'" +
                          " and clave = '" + clave + "' " +
                          " and Habilitado = 1";
            try
            {
                DataTable dt = new DataTable();
                dt = con.Ejecutarreader(sql);



                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine("entró reader " + Convert.ToString(dt.Rows[0][0].ToString()));

                    this.UsuarioID = Convert.ToInt32(dt.Rows[0][0].ToString());
                    this._Usuario = Convert.ToString(dt.Rows[0][1].ToString());
                    this.apellido = Convert.ToString(dt.Rows[0][4].ToString());
                    this.nombre = Convert.ToString(dt.Rows[0][3].ToString());
                    this.email = Convert.ToString(dt.Rows[0][6].ToString());
                    this.dni = Convert.ToInt32(dt.Rows[0][5].ToString());
                    this.habilitado = Convert.ToBoolean(dt.Rows[0][7].ToString());
                    this.FlagIntentosLogin = Convert.ToInt32(dt.Rows[0][8].ToString());

                }
                else
                {


                }

                UsuarioDAL_60MN usu = new UsuarioDAL_60MN(this.UsuarioID, this._Usuario, this.apellido, this.nombre, this.email, this.dni, this.habilitado, this.FlagIntentosLogin);

                return usu;

            }
            catch (Exception)
            {

                throw;
            }



        }

        public DataTable MostrarUsuarios()
        {

            string sql = "select UsuarioID, Usuario, Clave, Nombre, Apellido, DNI," +
                " Email, Habilitado, FlagIntentosLogin from Usuario ";

            dt = con.Ejecutarreader(sql);

            return dt;

        }

        public UsuarioDAL_60MN TraerDatosUsuario(string usuario)
        {
            string sql = "select UsuarioID,Usuario,Clave,Nombre,Apellido,DNI,Email,Habilitado,FlagIntentosLogin from Usuario where Usuario = '" + usuario + "'";

            try
            {

                dt = con.Ejecutarreader(sql);



                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine("entró reader " + Convert.ToString(dt.Rows[0][0].ToString()));

                    this.UsuarioID = Convert.ToInt32(dt.Rows[0][0].ToString());
                    this._Usuario = Convert.ToString(dt.Rows[0][1].ToString());
                    this.apellido = Convert.ToString(dt.Rows[0][4].ToString());
                    this.nombre = Convert.ToString(dt.Rows[0][3].ToString());
                    this.email = Convert.ToString(dt.Rows[0][6].ToString());
                    this.dni = Convert.ToInt32(dt.Rows[0][5].ToString());
                    this.habilitado = Convert.ToBoolean(dt.Rows[0][7].ToString());
                    this.FlagIntentosLogin = Convert.ToInt32(dt.Rows[0][8].ToString());

                }
                else
                {


                }

                UsuarioDAL_60MN usu = new UsuarioDAL_60MN(this.UsuarioID, this._Usuario, this.apellido, this.nombre, this.email, this.dni, this.habilitado, this.FlagIntentosLogin);

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
    }
}