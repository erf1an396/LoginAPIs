using loginAPP.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace loginAPP.Business.CQRS.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public string UserId { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
} 