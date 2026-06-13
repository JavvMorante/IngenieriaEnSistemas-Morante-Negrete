using Servicios_60MN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_60MN
{
    public class DigitosVerificadores_60MN
    {
        Encriptador_60MN crypt = new Encriptador_60MN();
        Conexion_60MN con = new Conexion_60MN();    
        DataTable dtUsuarios  = new DataTable();
        DataTable dtUsuarioOperacion = new DataTable();
        DataTable dtBitacora = new DataTable();

        public string VerificarDV()
        {
            try
            {
                DataTable dt = new DataTable();
                // este sql lo tengo que corregir para que coincida con mi base y crear los campos correspondientes
                string sql = " DECLARE @dvusuario INT;" +
              "   DECLARE @dvvusuario INT;" +
              "   DECLARE @dvusuariooperacion INT;" +
               "  DECLARE @dvvusuariooperacion INT;" +
              "   DECLARE @dvbitacora INT;" +
              "   DECLARE @dvvbitacora INT;" +
              "   DECLARE @dvperfil INT;" +
              "   DECLARE @dvvperfil INT;" +
              "   DECLARE @dvoperacion INT;" +
              "   DECLARE @dvvoperacion INT;" +


              "   select @dvusuario = SUM(CAST(DVH AS INT)) from usuario;" +
              "   select @dvvusuario = dvv from dvv where tabla like 'Usuario'; " +

              "   select @dvusuariooperacion = SUM(CAST(DVH AS INT)) from usuariooperacion; " +
              "  select @dvvusuariooperacion = dvv from dvv where tabla like 'usuariooperacion';" +

              "   select @dvbitacora = SUM(CAST(DVH AS INT)) from bitacora;" +
             "    select @dvvbitacora = dvv from dvv where tabla like 'bitacora';" +

              "   select @dvperfil = SUM(CAST(DVH AS INT)) from PerfilUsuario;" +
             "  select @dvvperfil = dvv from dvv where tabla like 'PerfilUsuario';" +


              "  select @dvoperacion = SUM(CAST(DVH AS INT)) from operacion;" +
              "   select @dvvoperacion = dvv from dvv where tabla like 'operacion';" +

              "   select" +
               "  case when @dvusuario = @dvusuario then 0 else 1 end as 'usuario'," +
               "  case when @dvusuariooperacion = @dvvusuariooperacion then 0 else 1 end as 'usuariooperacion'," +
               "  case when @dvvbitacora = @dvbitacora  then 0 else 1 end as 'bitacora'," +
               "  case when @dvvperfil = @dvperfil  then 0 else 1 end as 'perilusuario'," +
               " case when @dvoperacion = @dvvoperacion  then 0 else 1 end as 'operacion';";

                dt = con.Ejecutarreader(sql);

                string dvUsuario = "";
                string dvUsuarioOperacion = "";
                string dvBitacora = "";
                string dvPerfilUsuario = "";
                string dvOperacion = "";

                foreach (DataRow item in dt.Rows)
                {
                    dvUsuario = item[0].ToString();
                    dvUsuarioOperacion = item[1].ToString();
                    dvBitacora = item[2].ToString();
                    dvPerfilUsuario = item[3].ToString();
                    dvOperacion = item[4].ToString();

                }

                if (Convert.ToInt16(dvUsuario)==1)
                {
                    return "Fallé Calculo de la DV de Usuario, contacte al administrador";

                }
                if (Convert.ToInt16(dvUsuarioOperacion) == 1)
                {
                    return "Falló Calculo de DV de Patente-Usuario, contacte al administrador";

                }
                if (Convert.ToInt16(dvBitacora) == 1)
                {
                    return "Falló Calculo de DV de Bitacora, contacte al administrador";

                }
                if (Convert.ToInt16(dvPerfilUsuario) == 1)
                {
                    return "Falló Calculo de DV de Perfil de Usuario, contacte al administrador";

                }
                if (Convert.ToInt16(dvOperacion) == 1)
                {
                    return "Falló Calculo de DV de Patentes, contacte al administrador";

                }

                return "Dígitos calculados correctamente!";

            }
            catch (Exception ex)
            {
                return "Hubo problemas con los Digitos Verificadores: " + ex.Message;
            }
        }
        DataTable dtPerfil = new DataTable();
        DataTable dtOperacion = new DataTable();

        public string RecalcularDVH()
        {
            try
            {
                Thread tdv = new Thread(new ThreadStart(RecalcularDVHProcess));
                tdv.Start();

                return "ok";
            }

            catch (Exception ex) {
                return ex.Message;
        }
    }

        private void RecalcularDVHProcess()
        {
            con.Conectar();
            //usuarios
            string sql = "select UsuarioId, Usuario, Clave From usuario";
            dtUsuarios = con.Ejecutarreader(sql);

            string sqlUsop = "select UsuarioId, OperacionID, Habilitado from usuarioOperacion";
            dtUsuarioOperacion = con.Ejecutarreader(sqlUsop);

            string sqlBitacora = " select BitacoraID,UsuarioID,FechayHora from bitacora";
            dtBitacora = con.Ejecutarreader(sqlBitacora);

            string sqlPerfil = "  select PerfilUsuarioID,NombrePerfil,DescPerfil from PerfilUsuario";
            dtPerfil = con.Ejecutarreader(sqlPerfil);


            string sqlOperacion = "  select OperacionID,Descripcion,PatenteEscencial from Operacion";
            dtOperacion = con.Ejecutarreader(sqlOperacion);

            //ACTUALIZO DVH

            foreach (DataRow item in dtOperacion.Rows)
            {
                string concat = item[0].ToString() + item[1].ToString() + item[2].ToString();

                int flag = recalcularTablaDVH(concat);

                string sqlOp1 = "Update operacion set dvh = " + flag + "where OperacionId=" + item[0].ToString() + ";";

                con.Ejecutar(sqlOp1);
            }

            foreach (DataRow item in dtUsuarios.Rows)
            {
                string concat = item[0].ToString() + item[1].ToString() + item[2].ToString();

                int flag = recalcularTablaDVH(concat);

                string sql2 = "update Usuario set dvh = "+flag+" where UsuarioId= " + item[0].ToString() + ";";
                con.Ejecutar(sql2);
            }

            foreach (DataRow item in dtBitacora.Rows)
            {
                string concat = item[0].ToString() + item[1].ToString + item[2].ToString();

                int flag = RecalcularTablaDVH(concat);

                string sqlbit = "update Bitacora set dvh = " + flag + " where BitacoraId = " + item[0].ToString() + " " +
                    "and UsuarioId = " + item[1].ToString() + ";";

                con.Ejecutar(sqlbit);
            }

            foreach (DataRow item in dtPerfil.Rows)
            {
                string concat = item[0].ToString() + item[1].ToString() + item[2].ToString();
            }
                

            // continuar desde aca la op2
        }
}
