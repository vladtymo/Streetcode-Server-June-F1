using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Users
{
    public class UserDTO : UserRegisterDTO
    {
        public bool PhoneConfirmed { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }        
    }
}
