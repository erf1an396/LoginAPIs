using Microsoft.AspNetCore.Identity;

namespace loginAPP.Model
{
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
