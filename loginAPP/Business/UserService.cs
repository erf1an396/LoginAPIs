using loginAPP.DTO;
using loginAPP.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace loginAPP.Business
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUserAsync();
        Task<List<RoleDto>> GetRoleAsync();

        Task<bool> AddRoleAsync(string roleName);

        Task<bool> DeleteRoleAsync(string roleId);

        Task<bool> AssignRoleToUserAsync(string userId, string roleName);

        Task<bool> RemoveRoleFromUserAsync(string userId, string roleName);
        Task<bool> DeleteUserAsync(string userId);

        


    }



    public  class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userMananger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserService(UserManager<ApplicationUser> userManager , RoleManager<ApplicationRole> roleManager)
        {
            _userMananger = userManager;
            _roleManager = roleManager;
        }


        public async Task<List<UserDto>> GetUserAsync()
        {
            var users = await _userMananger.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();

            return users.Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()

            }).ToList();    
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userMananger.FindByIdAsync(userId);
            if(user == null) return false;

           var result = await _userMananger.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<List<RoleDto>> GetRoleAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(role => new RoleDto
            {
                Id = role.Id,
                roleName = role.Name
            }).ToList();
        }

        public async Task<bool> AddRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return false;
            var role = new ApplicationRole { Name = roleName };
            var result = await _roleManager.CreateAsync(role); ;

            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if(role == null)
                return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _userMananger.FindByIdAsync(userId);
            if(user == null) return false;

            var result = await _userMananger.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

       public async Task<bool> RemoveRoleFromUserAsync(string userId , string roleName)
        {
            var user = await _userMananger.FindByIdAsync(userId);
            if(user == null) return false;

            var result = await _userMananger.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }


        


    }


}
