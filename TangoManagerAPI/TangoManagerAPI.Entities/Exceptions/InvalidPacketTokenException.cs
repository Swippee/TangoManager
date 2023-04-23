using System;

namespace TangoManagerAPI.Entities.Exceptions
{
    public class InvalidPacketTokenException : Exception
    {
        public InvalidPacketTokenException(string msg) : base(msg)
        {

        }
    }
}
