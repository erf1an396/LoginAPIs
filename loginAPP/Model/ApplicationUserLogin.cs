using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace loginAPP.Model
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        [Key]
        public virtual ApplicationUser User { get; set; }
    }
}
