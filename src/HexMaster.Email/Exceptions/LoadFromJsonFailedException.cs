using System;

namespace HexMaster.Email.Exceptions
{
    public class LoadFromJsonFailedException : Exception
    {
        internal LoadFromJsonFailedException(Exception? inner = null) : base($"Could not restore the MailMessage object by deserialization from JSON", inner)
        {

        }
    }
}