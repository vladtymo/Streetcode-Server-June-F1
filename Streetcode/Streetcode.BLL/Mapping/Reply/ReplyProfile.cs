using AutoMapper;
using Streetcode.BLL.DTO.Comment;
using Streetcode.DAL.Entities.Comments;
namespace Streetcode.BLL.Mapping.Reply;

public class ReplyProfile : Profile
{
    public ReplyProfile()
    {
        CreateMap<ReplyCreateDTO, DAL.Entities.Comments.Reply>().ReverseMap();
    }
}