using System.Collections.Generic;
using System.Text.Json;
using HexMaster.Email.Exceptions;

namespace HexMaster.Email.DomainModels
{
    public class MailMessage
    {

        private List<Recipient> _recipients;
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public Sender Sender { get; private set; }
        public IReadOnlyCollection<Recipient> Recipients => _recipients.AsReadOnly();


        public string SerializeToJson()
        {
            var opts = new JsonSerializerOptions();
            opts.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opts.PropertyNameCaseInsensitive = true;
            return JsonSerializer.Serialize(this , opts);
        }

        public static MailMessage FromJson(string json)
        {
            var deserializedObject =  JsonSerializer.Deserialize<MailMessage>(json);
            if (deserializedObject != null)
            {
                return deserializedObject;
            }

            throw new LoadFromJsonFailedException();
        }

        public MailMessage( Sender sender, Recipient recipient, string subject, string body )
        {
            Sender = sender;
            _recipients = new List<Recipient> { recipient };
            Subject = subject;
            Body = body;
        }
    }
}