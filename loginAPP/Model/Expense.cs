using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace loginAPP.Model
{


    [Table("Expenses")]
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime Date {  get; set; }

    }
}
