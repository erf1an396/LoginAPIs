using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace loginAPP.Model
{


    [Table("Expenses")]
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        
        public Guid UserId { get; set; }

        public decimal Amount { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public DateTime Date {  get; set; } = DateTime.Now;


        public ApplicationUser ApplicationUser { get; set; }
        

    }
}
