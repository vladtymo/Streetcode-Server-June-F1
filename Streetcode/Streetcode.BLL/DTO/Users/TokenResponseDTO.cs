using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Users
{
    public class TokenResponseDTO
    {
        public RefreshTokenDTO RefreshToken { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
