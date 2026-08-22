using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_60MN
{
    public class IdiomaBLL_60MN
    {
        string ISeleccionado = "";
        public IdiomaBLL_60MN() { }
        public int IdiomaID { get; set; }
        public string Descripcion { get; set; }

        public string CargarIdioma()
        {
            DAL_60MN.IdiomaDAL_60MN _idioma = new DAL_60MN.IdiomaDAL_60MN();
            ISeleccionado = _idioma.CargarIdioma();



            return ISeleccionado;
        }

        public string SetearIdioma(int idiomaID)
        {
            DAL_60MN.IdiomaDAL_60MN idioma1 = new DAL_60MN.IdiomaDAL_60MN();

            this.Descripcion = idioma1.SetearIdioma(idiomaID);


            return this.Descripcion;


        }
    }
}

