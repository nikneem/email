using System;

namespace HexMaster.Email.Exceptions
{
    [Serializable]
    public class RecipientAlreadyExistsException : Exception
    {
        internal RecipientAlreadyExistsException(string email) : base(
            $"A recipient with email address {email} is already present")
        {

        }
    }
}