using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoManagerAPI.Entities.Ports.Exceptions
{
    public abstract class TangoManagerException : Exception
    {
        protected TangoManagerException(string message)
            : base(message)
        {

        }
    }
}
