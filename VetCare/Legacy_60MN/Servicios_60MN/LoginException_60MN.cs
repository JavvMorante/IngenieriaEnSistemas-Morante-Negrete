using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios_60MN
{
    public class LoginException_60MN : Exception
    {
        public LoginResult_60MN Result;
        public LoginException_60MN(LoginResult_60MN result)
        
        { 
            Result = result;
        
        }
    }
}
