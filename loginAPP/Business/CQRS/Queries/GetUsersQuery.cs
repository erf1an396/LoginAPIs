using loginAPP.DTO;
using loginAPP.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace loginAPP.Business.CQRS.Queries
{
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUsersQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync(cancellationToken);

            return users.Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            }).ToList();
        }
    }
} 