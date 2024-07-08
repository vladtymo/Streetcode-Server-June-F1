using Microsoft.AspNetCore.Http;

namespace Streetcode.BLL.Interfaces.Email
{
    public interface ISendVerificationEmail
    {
        public Task SendVerification(string email);
    }
}
