using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_60MN.Seguridad_MN60
{
    public class BitacoraBLL_60MN
    {
        public string Accion { get; set; }
        public int Criticidad { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechayHora { get; set; }
        public string NombreOperacion { get; set; }
        // public int Usuarioid { get; set = 0; }
        public int Usuarioid { get; set; }

        public DataTable ConsultarBitacora(string fechadesde, string fechahasta, string sqlcriticidad, string sqlusuario)
        {
            DAL_60MN.BitacoraDAL_60MN logDAL = new DAL_60MN.BitacoraDAL_60MN();
            return logDAL.ConsultarBitacora(fechadesde, fechahasta, sqlcriticidad, sqlusuario);
        }

        public void Encriptar()
        {
            throw new System.NotImplementedException();
        }

        public void ExportarBitacora()
        {
            throw new System.NotImplementedException();
        }

        public void ImportarBitacora()
        {
            throw new System.NotImplementedException();
        }

        public DataTable traerUsuarios()
        {
            DataTable datausuario = new DataTable();
        DAL_60MN.BitacoraDAL_60MN log = new DAL_60MN.BitacoraDAL_60MN();
            datausuario = log.traerUsuarios();
            return datausuario;


        }

        public DataTable traerCriticidad()
        {
            DataTable dataCriticidad = new DataTable();
            DAL_60MN.BitacoraDAL_60MN log = new DAL_60MN.BitacoraDAL_60MN();
            dataCriticidad = log.traerCriticidad();
            return dataCriticidad;


        }

        public string IngresarDatoBitacora(string NombreOperacion, string Descripcion, int Criticidad, int Usuarioid)
        {


            DAL_60MN.BitacoraDAL_60MN log = new DAL_60MN.BitacoraDAL_60MN ();


            string rta = log.IngresarDatoBitacora(NombreOperacion, Descripcion, Criticidad, Usuarioid);

            return rta;

        }

        public void Exportar_a_Excel()
        {
            throw new System.NotImplementedException();
        }
    }
}

