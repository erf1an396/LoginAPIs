using loginAPP.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace loginAPP.Business.CQRS.Commands
{
    public class DeleteRoleCommand : IRequest<bool>
    {
        public string RoleId { get; set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DeleteRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == null)
                return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
    }
} 