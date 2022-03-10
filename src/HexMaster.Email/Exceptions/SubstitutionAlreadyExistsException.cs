using System;

namespace HexMaster.Email.Exceptions
{
    [Serializable]
    public class SubstitutionAlreadyExistsException : Exception
    {
        public SubstitutionAlreadyExistsException(string key) : base($"A substitution with key {key} already exists")
        {
        }
    }
}