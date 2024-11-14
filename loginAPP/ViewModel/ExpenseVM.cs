namespace loginAPP.ViewModel
{
    public class ExpenseVM
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
