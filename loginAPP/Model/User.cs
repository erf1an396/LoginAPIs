using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace loginAPP.Model
{

    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

    }
}
