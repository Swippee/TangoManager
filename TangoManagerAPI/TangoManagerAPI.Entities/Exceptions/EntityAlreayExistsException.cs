using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Exceptions;

namespace TangoManagerAPI.Entities.Exceptions
{
    public class EntityAlreadyExistsException : TangoManagerException
    {
        public EntityAlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}
