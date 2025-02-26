namespace loginAPP.ViewModel
{
    public class ExpenseSummaryVM
    {
        public List<ExpenseVM> Transactions { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
    }
}
