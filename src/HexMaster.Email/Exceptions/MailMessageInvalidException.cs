using System;

namespace HexMaster.Email.Exceptions
{
    [Serializable]
    public class MailMessageInvalidException : Exception
    {
        public MailMessageInvalidException() : base("The mail message is invalid. Is must have a subject, at least one body and at least one recipient")
        {
        }
    }
}