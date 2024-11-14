namespace loginAPP.ViewModel
{
    public class UserVM
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public List<ExpenseVM> Expenses { get; set; }
    }
}
