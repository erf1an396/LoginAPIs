using loginAPP.DTO;
using loginAPP.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace loginAPP.Business.CQRS.Queries
{
    public class GetRolesQuery : IRequest<List<RoleDto>>
    {
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RoleDto>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GetRolesQueryHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles.ToListAsync(cancellationToken);
            return roles.Select(role => new RoleDto
            {
                Id = role.Id,
                roleName = role.Name
            }).ToList();
        }
    }
} 