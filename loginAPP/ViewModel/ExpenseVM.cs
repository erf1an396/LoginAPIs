using System.Text.Json.Serialization;

namespace loginAPP.ViewModel
{
    public class ExpenseVM
    {
        public  int Id { get; set; }
        public Guid UserId { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
