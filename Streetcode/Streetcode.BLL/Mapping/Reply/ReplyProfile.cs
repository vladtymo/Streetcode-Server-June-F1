using AutoMapper;
using Streetcode.BLL.DTO.Comment;
using Streetcode.DAL.Entities.Comments;
namespace Streetcode.BLL.Mapping.Reply;

public class ReplyProfile : Profile
{
    public ReplyProfile()
    {
        CreateMap<ReplyCreateDTO, DAL.Entities.Comments.Reply>()
            .ForMember(dest => dest.CommentContent, opt => opt.MapFrom(src => src.CommentContent))
            .ForMember(dest => dest.StreetcodeId, opt => opt.MapFrom(src => src.StreetcodeId))
            .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId));
    }
}