using System;

namespace HexMaster.Email.Exceptions
{
    [Serializable]
    public class InvalidEmailAddressException : Exception
    {
        internal InvalidEmailAddressException(string emailAddress, Exception? inner = null) : base(
            $"The email address {emailAddress} is not a valid email address", inner)
        {

        }
    }
}