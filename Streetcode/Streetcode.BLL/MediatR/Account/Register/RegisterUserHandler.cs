using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Account.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<UserDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly UserManager<User> _userManager;
        public RegisterUserHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<UserDTO>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.newUser);
            var result = await _userManager.CreateAsync(user, request.newUser.Password);
            if (!result.Succeeded)
            {
                var errorMsg = "User creation failed";
                _logger.LogError(user, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
            
            await _userManager.AddToRoleAsync(user, "User");
            await _userManager.UpdateAsync(user);
            return Result.Ok(_mapper.Map<UserDTO>(user));
        }
    }
}
