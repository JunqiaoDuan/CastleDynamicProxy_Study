using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreezeExample.Exceptions
{
    public class NotFreezableObjectException : Exception
    {
        public NotFreezableObjectException()
        {
            
        }

        public NotFreezableObjectException(object objectValue)
        {
            
        }
    }
}
