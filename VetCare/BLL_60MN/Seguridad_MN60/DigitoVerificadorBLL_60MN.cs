using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_60MN.Seguridad_MN60
{
    public class DigitoVerificadorBLL_60MN
    {
        int contadorDVV = 0;
        int contadorDVH = 0;
        DAL_60MN.DigitosVerificadores_60MN digitos = new DAL_60MN.DigitosVerificadores_60MN();
        string rta = "";

        public string CalcularDigitosVerificadores()
        {
            rta = digitos.VerificarDV();

            return rta;
        }

        public void CalcularDVV()
        {

        }

        public int CarlcularDVH(string aux)
        {
            char[] charArray = aux.ToCharArray();

            foreach (char ch in charArray)
            {
                contadorDVV += (int)ch;
            }

            return contadorDVV;
        }

        public string RecalcularDVH()
        {


            rta = digitos.RecalcularDVH();
            return rta;
        }
    }

}

