using System;

namespace HexMaster.Email.Exceptions
{
    [Serializable]
    public class LoadFromJsonFailedException : Exception
    {
        internal LoadFromJsonFailedException(Exception? inner = null) : base($"Could not restore the Message object by deserialization from JSON", inner)
        {

        }
    }
}