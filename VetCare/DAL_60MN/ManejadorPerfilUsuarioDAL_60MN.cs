using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_60MN
{
    public class ManejadorPerfilUsuarioDAL_60MN
    {
        DigitosVerificadores_60MN dv = new DigitosVerificadores_60MN();

        Conexion_60MN con = new Conexion_60MN();
        public List<string> MostrarMenuperfiles(int usuarioid)
        {
            List<string> operacionesusuario = new List<string>();
            DataTable dt = new DataTable();

            dt = this.traeroperacionesusuario(usuarioid);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                { operacionesusuario.Add(dt.Rows[i]["Descripcion"].ToString()); }

            }

            return operacionesusuario;
        }

        internal DataTable traeroperacionesusuario(int Usuarioid)
        {
            DataTable dt = new DataTable();

            string sql = "select o.Descripcion " +
                " from usuariooperacion uo INNER JOIN OPERACION o ON O.OperacionID = UO.OperacionID " +
                 " where uo.UsuarioID = " + Usuarioid + " and uo.Habilitado = 'S' ";

            con.Conectar();


            con.Ejecutarreader(sql);
            dt = con.Ejecutarreader(sql);




            con.Desconectar();


            return dt;
        }

        public void BloqueaOperacionUsuario(string nombreUsuario, string patente)
        {
            string sql = "update usuariooperacion set Habilitado = 'N' " +
                " where Usuarioid = (select UsuarioID from usuario where Usuario like '%" + nombreUsuario + "%')" +
                " and Operacionid = (select OperacionID from OPERACION where Descripcion like '%" + patente + "%')";

            con.Ejecutar(sql);
            dv.RecalcularDVH();
        }

        /// <summary>
        ///hacer la verificacion si existe familia antes de dar el alta
        /// </summary>
        public int VerificarAltafamilia(string nombrePerfil)
        {
            string sql = " select case when count(NombrePerfil) <> 0 then 1 else 0  end 'columna'  from PerfilUsuario where NombrePerfil like '" + nombrePerfil + "'";
            int rta = 0;
            DataTable dt = new DataTable();
            con.Conectar();
            con.Ejecutarreader(sql);
            dt = con.Ejecutarreader(sql);
            con.Desconectar();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                { rta = Convert.ToInt16(dt.Rows[i]["columna"].ToString()); }

            }

            return rta;
        }

        public void DesbloqueaOperacionaUsuario(string nombreUsuario, string patente)
        {
            string sql = "update usuariooperacion set Habilitado = 'S' " +
                           " where Usuarioid = (select UsuarioID from usuario where Usuario like '%" + nombreUsuario + "%')" +
                           " and Operacionid = (select OperacionID from OPERACION where Descripcion like '%" + patente + "%')";

            con.Ejecutar(sql);
            dv.RecalcularDVH();
        }

        public string ModificarPerfilUsuario(string nombrePerfil, string descPerfil, int perfilID)
        {
            string rta;
            string sql = "Update perfilusuario set NombrePerfil = '" + nombrePerfil + "',DescPerfil= '" + descPerfil + "'" +
                 " where PerfilUsuarioID = " + perfilID + ";";

            try
            {
                con.Ejecutar(sql);
                dv.RecalcularDVH();
                return rta = "True";
            }

            catch (Exception ex)
            {
                return rta = ex.Message;
                dv.RecalcularDVH();
                throw;
            }
        }

        public string verificarPatentesBloqueo(string nombreUsuario, string patente)
        {
            string rta = "";

            string sql = "select Descripcion from operacion where operacionid in " +
" (select distinct OperacionID from usuariooperacion " +
" where UsuarioID = (select usuarioid from usuario where Usuario like '%" + nombreUsuario + "%')" +
" EXCEPT" +
" select distinct OperacionID from usuariooperacion " +
" WHERE OperacionID IN(select distinct OperacionID from usuariooperacion" +
" where UsuarioID <> (select usuarioid from usuario where Usuario like '%" + nombreUsuario + "%') " + " ))" +
" and Descripcion like '" + patente + "'";

            DataTable DT = con.Ejecutarreader(sql);


            if (DT.Rows.Count > 0)
            {


                rta = DT.Rows[0][0].ToString();


                return rta;
            }
            else
            {

                rta = "True";
                return rta;

            }



        }

        public string EliminarPerfilUsuario(int perfilID)
        {
            string rta = "";

            string sql = "Delete perfilusuario where PerfilUsuarioID = " + perfilID + ";" +
                       "Delete perfiloperacion where PerfilUsuarioID = " + perfilID + ";";

            try
            {
                con.Ejecutar(sql);
                dv.RecalcularDVH();
                con.Desconectar();

                return rta = "True";
            }
            catch (Exception ex)
            {
                return rta = ex.Message;
            }

        }

        public string _CrearPerfilUsuario(string nombrePerfil, string descPerfil)
        {
            string sql = "insert into PerfilUsuario(NombrePerfil,DescPerfil,DVH) values('" + nombrePerfil + "','" + descPerfil + "',NULL)";


            string rta = con.Ejecutar(sql);
            dv.RecalcularDVH();
            return rta;
        }

        public List<string> MostrarListaOperaciones()
        {
            List<string> listaoperaciones = new List<string>();
            DataTable dt = new DataTable();

            string sql = "select Descripcion from OPERACION";

            dt = con.Ejecutarreader(sql);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                { listaoperaciones.Add(dt.Rows[i]["Descripcion"].ToString()); }

            }

            return listaoperaciones;

        }

        public List<string> MostrarListaOperaciones(int perfilID)
        {
            List<string> listaoperaciones1 = new List<string>();
            listaoperaciones1.Clear();
            DataTable dt = new DataTable();

            string sql = "Select op.Descripcion from perfiloperacion po inner join " +
                " operacion op on op.OperacionID = po.Operacionid " +
                " where PerfilUsuarioID = " + perfilID;

            dt = con.Ejecutarreader(sql);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                { listaoperaciones1.Add(dt.Rows[i]["Descripcion"].ToString()); }

            }

            return listaoperaciones1;
        }
        public List<string> MostrarListaOperaciones(string Perfil)
        {
            List<string> listaoperaciones1 = new List<string>();
            listaoperaciones1.Clear();
            DataTable dt = new DataTable();

            string sql = " Select op.Descripcion from perfiloperacion po inner join" +
                 " operacion op on op.OperacionID = po.Operacionid " +
                 " where PerfilUsuarioID = (select PerfilUsuarioID from PerfilUsuario" +
                 " where NombrePerfil like '%" + Perfil + "%');";


            dt = con.Ejecutarreader(sql);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                { listaoperaciones1.Add(dt.Rows[i]["Descripcion"].ToString()); }

            }

            return listaoperaciones1;
        }

        public int AsignarUsuarioaPerfil(string nombreperfil, string nombreUsuario)
        {

            string sql = "  Delete usuariofamilia where UsuarioID = ( " +
                         "  select UsuarioID from usuario where usuario like '%" + nombreUsuario + "%');" +
                         "  insert into usuariofamilia(PerfilID, UsuarioID, Actual) " +
                         "  select PerfilUsuarioID, UsuarioID,1 as Actual  from usuario, PerfilUsuario " +
                         "  where PerfilUsuarioID in (select PerfilUsuarioID from PerfilUsuario where NombrePerfil like '%" + nombreperfil + "%') " +
                         "  and usuarioID in (select UsuarioID from usuario where usuario like '%" + nombreUsuario + "%') ; ";
            try
            {
                con.Conectar();
                con.Ejecutar(sql);
                dv.RecalcularDVH();
                return 0;
            }
            catch (Exception)
            {

                return 1;
            }


        }

        public void AsignarOperacionesalPerfil(int perfilID, List<string> listaoperacionesperfil)
        {
            //1er sql
            string primersql = "delete perfiloperacion where PerfilUsuarioID = " + perfilID;
            con.Ejecutar(primersql);
            con.Desconectar();

            con.Conectar();

            string sql;
            foreach (string item in listaoperacionesperfil)
            {
                sql = "INSERT INTO perfiloperacion (OperacionID,PerfilUsuarioID)" +
                      " SELECT OperacionID, " + perfilID + " " +
                      " FROM OPERACION op" +
                      " WHERE op.Descripcion like '" + item.ToString() + "';";

                con.Ejecutar(sql);
                dv.RecalcularDVH();
            }
            con.Desconectar();


        }
        public void AsignarOperacionesalPerfil(string NombreUsuario, List<string> listaoperacionesperfil)
        {
            //1er sql
            string primersql = " delete usuariooperacion where usuarioid = (select Usuarioid " +
                                " from usuario where Usuario like '" + NombreUsuario + "'); ";
            con.Ejecutar(primersql);
            con.Desconectar();

            con.Conectar();

            string sql;
            foreach (string item in listaoperacionesperfil)
            {
                sql = " INSERT INTO usuariooperacion (UsuarioID,OperacionID,Habilitado)" +
                      " SELECT usu.UsuarioID,op.OperacionID,'S' as Habilitado" +
                      " FROM OPERACION op, Usuario usu" +
                      " WHERE op.Descripcion like '" + item.ToString() + "'" +
                      " and usu.Usuario like '" + NombreUsuario + "';";

                con.Ejecutar(sql);
            }
            dv.RecalcularDVH();
            con.Desconectar();


        }

        public DataTable BuscarPerfilUsuarios()
        {
            DataTable dt = new DataTable();

            string sql = "Select PerfilUsuarioID,NombrePerfil,DescPerfil,DVH from perfilusuario";

            con.Conectar();

            con.Ejecutarreader(sql);
            dt = con.Ejecutarreader(sql);


            con.Desconectar();

            return dt;


        }
        public string verificarPatentesEscenciales(string NombreUsuario)
        {
            string rta = "";

            string sql = "select Descripcion from operacion where operacionid in" +
" (select distinct OperacionID from usuariooperacion " +
" where UsuarioID = (select usuarioid from usuario where Usuario like '%" + NombreUsuario + "%')" +
" EXCEPT" +
" select distinct OperacionID from usuariooperacion " +
" WHERE OperacionID IN(select distinct OperacionID from usuariooperacion" +
" where UsuarioID <> (select usuarioid from usuario where Usuario like '%" + NombreUsuario + "%') " + " ))";

            DataTable DT = con.Ejecutarreader(sql);


            if (DT.Rows.Count > 0)
            {



                foreach (DataRow row in DT.Rows)
                {

                    rta = rta + "--" + row[0].ToString() + "--";
                }

                return rta;
            }
            else
            {

                rta = "True";
                return rta;

            }

        }

        public string verificarPatentesEscenciales(int usuarioID)
        {
            string rta = "";

            string sql = "select Descripcion from operacion where operacionid in" +
" (select distinct OperacionID from usuariooperacion " +
" where UsuarioID = " + usuarioID +
" EXCEPT" +
" select distinct OperacionID from usuariooperacion " +
" WHERE OperacionID IN(select distinct OperacionID from usuariooperacion" +
" where UsuarioID <>" + usuarioID + " ))";

            DataTable DT = con.Ejecutarreader(sql);


            if (DT.Rows.Count > 0)
            {



                foreach (DataRow row in DT.Rows)
                {



                    rta = rta + "--" + row[0].ToString() + "--";
                }

                return rta;

            }
            else
            {

                rta = "True";
                return rta;

            }



        }
    }
}
    

