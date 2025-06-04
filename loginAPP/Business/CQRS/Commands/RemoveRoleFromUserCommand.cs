using loginAPP.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace loginAPP.Business.CQRS.Commands
{
    public class RemoveRoleFromUserCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }

    public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RemoveRoleFromUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return false;

            var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
            return result.Succeeded;
        }
    }
} 