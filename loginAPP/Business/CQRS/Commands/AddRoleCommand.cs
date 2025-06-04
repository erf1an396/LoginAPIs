using loginAPP.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace loginAPP.Business.CQRS.Commands
{
    public class AddRoleCommand : IRequest<bool>
    {
        public string RoleName { get; set; }
    }

    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, bool>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AddRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            if (await _roleManager.RoleExistsAsync(request.RoleName))
                return false;

            var role = new ApplicationRole { Name = request.RoleName };
            var result = await _roleManager.CreateAsync(role);

            return result.Succeeded;
        }
    }
} 