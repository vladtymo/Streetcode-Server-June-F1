using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.AdditionalContent.Email
{
    public class EmailConfiguration : IEntity
    {
        public string From { get; set; } = string.Empty;
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
