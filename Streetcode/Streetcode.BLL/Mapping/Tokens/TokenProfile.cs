using AutoMapper;
using Streetcode.BLL.DTO.Users;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Mapping.Tokens
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<RefreshToken, RefreshTokenDTO>()
                .ForPath(dest => dest.Token, conf => conf.MapFrom(src => src.Token))
                .ForPath(dest => dest.Created, conf => conf.MapFrom(src => src.Created))
                .ForPath(dest => dest.Expires, conf => conf.MapFrom(src => src.Expires))
                .ReverseMap();
        }
    }
}
