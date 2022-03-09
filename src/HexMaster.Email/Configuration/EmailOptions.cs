namespace HexMaster.Email.Configuration
{
    public class EmailOptions
    {

        public const string SectionName = "EmailService";

        public string SmtpHost { get; set; } = default!;
        public int? SmtpPort{ get; set; }
        public bool? UseSsl { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

    }
}