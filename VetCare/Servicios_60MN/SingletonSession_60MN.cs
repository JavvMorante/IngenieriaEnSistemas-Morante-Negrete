using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Servicios_60MN
{
    public class SingletonSession_60MN
    {

        private static Session_60MN _instancia;
        private static Object _lock = new object();

        public static Session_60MN Instancia
        {
            get
            {
                lock (_lock)
                {
                    if (_instancia == null)
                        _instancia = new Session_60MN();
                }

                return _instancia;
            }
        }

    }
}
