using AutoMapper;
using Streetcode.BLL.DTO.Users;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Mapping.Tokens
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<RefreshToken, RefreshTokenDTO>().ReverseMap();
        }
    }
}
