using Microsoft.AspNetCore.Identity;

namespace loginAPP.Model
{
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
