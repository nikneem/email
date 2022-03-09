using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HexMaster.Email.Exceptions;

namespace HexMaster.Email.DomainModels
{
    public class Message
    {

        private List<Recipient> _recipients;
        private List<Body> _bodies;
        public string Subject { get; private set; }
        public Sender Sender { get; private set; }
        public IReadOnlyCollection<Recipient> Recipients => _recipients.AsReadOnly();
        public IReadOnlyCollection<Body> Bodies => _bodies.AsReadOnly();

        public void SetSubject(string subject)
        {
            Subject = subject;
        }

        public void AddRecipient(Recipient value)
        {
            if (_recipients.Any(r => r.EmailAddress .Equals( value.EmailAddress, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new RecipientAlreadyExistsException(value.EmailAddress);
            }
            _recipients.Add(value);
        }
        public void RemoveRecipient(Recipient value)
        {
            RemoveRecipient(value.EmailAddress);
        }
        public void RemoveRecipient(string value)
        {
            _recipients.RemoveAll(r => r.EmailAddress.Equals(value, StringComparison.CurrentCultureIgnoreCase));
        }

        public void AddBody(Body value)
        {
            if (_bodies.Any(b => b.Name.Equals(value.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new BodyAlreadyExistsException(value.Name);
            }
            _bodies.Add(value);
            if (_bodies.Count == 1)
            {
                value.SetDefault(true);
            }
        }
        public void RemoveBody(Body value)
        {
            RemoveBody(value.Name);
        }
        public void RemoveBody(string value)
        {
            _bodies.RemoveAll(b => b.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsValid()
        {
            var valid = !string.IsNullOrWhiteSpace(Subject);
            valid &= Recipients.Count > 0;
            valid &= Bodies.Count > 0;
            return valid;
        }

        public string SerializeToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public async Task<Stream> SerializeToStreamAsync()
        {
            var json = SerializeToJson();
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);

            await streamWriter.WriteAsync(json);
            await streamWriter.FlushAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static Message FromJson(string json)
        {
            var deserializedObject = JsonSerializer.Deserialize<Message>(json);
            if (deserializedObject != null)
            {
                return deserializedObject;
            }

            throw new LoadFromJsonFailedException();
        }
        public static async Task<Message> FromStreamAsync(Stream input)
        {
            if (input.CanRead && input.CanSeek)
            {
                input.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(input);
                var json = await streamReader.ReadToEndAsync();
                return FromJson(json);
            }

            throw new LoadFromStreamFailedException();
        }

        public Message(
            Sender sender, 
            Recipient recipient, 
            string subject, 
            Body body)
            : this(sender, new[] {recipient}, subject, new[] {body})
        {
        }

        [JsonConstructor]
        public Message(
            Sender sender, 
            IReadOnlyCollection<Recipient> recipients, 
            string subject,
            IReadOnlyCollection<Body> bodies)
        {
            Sender = sender;
            _recipients = recipients.ToList();
            Subject = subject;
            _bodies = bodies.ToList();
        }
    }
}