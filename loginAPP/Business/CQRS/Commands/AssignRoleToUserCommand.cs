using loginAPP.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace loginAPP.Business.CQRS.Commands
{
    public class AssignRoleToUserCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }

    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignRoleToUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;     
        }

        public async Task<bool> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return false;

            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            return result.Succeeded;
        }
    }
} 