﻿using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Identity.Users.Commands
{
    public class UpdateUserStatusCommand : IRequest<IResponseWrapper>
    {
        public ChangeUserStatusRequest ChangeUserStatus { get; set; }
    }

    public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand, IResponseWrapper>
    {
        private readonly IUserService _userService;
        public async Task<IResponseWrapper> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userService
                .ActivateOrDeactivateAsync(request.ChangeUserStatus.UserId, request.ChangeUserStatus.Activation);
            return await ResponseWrapper<string>
                .SuccessAsync(data: userId, message: request.ChangeUserStatus.Activation ?
                "User activated successfully." : "User deactivated successfully.");
        }
    }
}
