using System;

namespace TangoManagerAPI.Entities.Exceptions
{
    public class InvalidPacketTokenHeaderException : Exception
    {
        public InvalidPacketTokenHeaderException(string msg) : base(msg)
        {

        }
    }
}
