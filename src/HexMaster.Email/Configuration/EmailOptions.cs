namespace HexMaster.Email.Configuration
{
    public class EmailOptions
    {
        public string SmtpHost { get; set; }
        public int? SmtpPort{ get; set; }
        public bool? UseSsl { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

    }
}