using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.MediatR.Account.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<DeleteUserResponceDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly UserManager<User> _userManager;

        public DeleteUserCommandHandler(IMapper mapper, ILoggerService logger, UserManager<User> userManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<DeleteUserResponceDto>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            Guid userId = Guid.Parse(request.Dto.Id!);

            User? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                var error = string.Format(MessageResourceContext.GetMessage(ErrorMessages.UserWithIdNotFound), userId);

                _logger.LogError(request, error);

                return Result.Fail(error);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var error = result.Errors.Select(e => e.Description).Aggregate((e1, e2) => "- " + e1 + "\n" + "- " + e2 + "\n");

                _logger.LogError(request, error);

                return Result.Fail(error);
            }

            return Result.Ok(_mapper.Map<DeleteUserResponceDto>(user));
        }
    }
}
