using Microsoft.AspNetCore.Identity;

namespace loginAPP.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }


        

        

        public string Email { get; set; }

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    }
}
