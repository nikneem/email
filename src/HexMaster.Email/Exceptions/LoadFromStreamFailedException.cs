using System;

namespace HexMaster.Email.Exceptions
{
    public class LoadFromStreamFailedException : Exception
    {
        internal LoadFromStreamFailedException(Exception? inner = null) : base($"Could not restore the Message object by deserialization from Stream", inner)
        {

        }
    }
}