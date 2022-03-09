using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using HexMaster.Email.Exceptions;

namespace HexMaster.Email.DomainModels
{
    public class Sender
    {
        public string EmailAddress { get; private set; }
        public string? Name { get; private set; }
        public string? ReplyToAddress { get; private set; }

        public void SetName(string value)
        {
            Name = value;
        }

        public void SetEmailAddress(string value)
        {
            if (!Regex.IsMatch(value, RegularExpression.Email))
            {
                throw new InvalidEmailAddressException(value);
            }

            EmailAddress = value;
        }

        public void SetReplyToAddress(string value)
        {
            if (!Regex.IsMatch(value, RegularExpression.Email))
            {
                throw new InvalidEmailAddressException(value);
            }

            ReplyToAddress = value;
        }

        [JsonConstructor]
        public Sender(string emailAddress, string? name = null, string? replyToAddress = null)
        {
            EmailAddress = emailAddress;
            Name = name ?? emailAddress;
            ReplyToAddress = replyToAddress;
        }

    }
}