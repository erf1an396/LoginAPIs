using loginAPP.Context;
using loginAPP.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using loginAPP.Model;
using Microsoft.AspNetCore.Identity;

namespace loginAPP.Business
{

    public interface IExpense
    {
        Task<List<ExpenseVM>> GetExpensesAsync();

        Task<ExpenseVM> AddExpenseAsync(ExpenseVM model);

        Task<List<ExpenseVM>> GetExpenseByIdAsync(string userId);
        Task<ExpenseSummaryVM> GetExpenseSummaryAsync(string userId);

        Task<bool> DeleteExpenseAsync(int id);

        Task<List<string>> GetRoleByIdAsync(string userId);

        Task<List<MontlyFinanceVM>> GetMontlyFinancesAsync(string userId);
    }

    public class Expense : IExpense
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public Expense(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<List<ExpenseVM>> GetExpensesAsync()
        {
           var resutl =  await _db.Expenses
                .Select(c => new ExpenseVM()
           {
               Id = c.Id,
               Date = c.Date,
               Description = c.Description,
               Amount = c.Amount,
               UserId = c.UserId,
               Type = c.Type,

           }).ToListAsync();



            return resutl;

        }

        public async Task<List<ExpenseVM>> GetExpenseByIdAsync(string userId)
        {
            return await _db.Expenses.Where(e => e.UserId == new Guid(userId)).Select(e => new ExpenseVM
            {
                Id = e.Id,
                Date = e.Date,
                Description= e.Description,
                Amount = e.Amount,
                UserId = e.UserId,
                Type = e.Type,


            }).ToListAsync();
        }

        public async Task<ExpenseSummaryVM> GetExpenseSummaryAsync(string userId)
        {
            var expenses = await GetExpenseByIdAsync(userId);

            decimal totalIncome = expenses.Where(e => e.Type == "واریز").Sum(e => e.Amount);
            decimal totalExpense = expenses.Where(e => e.Type == "برداشت").Sum(e => e.Amount);
            decimal balance = totalIncome - totalExpense;

            return new ExpenseSummaryVM
            {
                Transactions = expenses,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = balance
            };
        }

        public async Task<List<MontlyFinanceVM>> GetMontlyFinancesAsync(string userId)
        {
            var montlyFinace = await _db.Expenses.Where(e => e.UserId == new Guid(userId)).GroupBy(t => new {t.Date.Year , t.Date.Month }).Select(g => new MontlyFinanceVM
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalRevenue = g.Where(e=> e.Type == "واریز").Sum(e => e.Amount),
                TotalExpense = g.Where(e => e.Type == "برداشت").Sum(e => e.Amount),
            }).ToListAsync();

            return montlyFinace;
        }



        

        public async Task<ExpenseVM> AddExpenseAsync(ExpenseVM model)
        {
            

            var expense = new Model.Expense()
            {
                UserId = model.UserId,
                Description = model.Description,
                Date = model.Date,
                Amount = model.Amount,
                Type = model.Type,
            };

            _db.Expenses.Add(expense);
            await _db.SaveChangesAsync();

            

            var expenseVM = new ExpenseVM()
            {
                UserId = expense.UserId,
                Description = expense.Description,
                Date = expense.Date,
                Amount = expense.Amount,
                Type = expense.Type,
            };
            return expenseVM;

        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _db.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            if (expense == null)
                return false;

            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetRoleByIdAsync(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var result = await _userManager.GetRolesAsync(user);
            return result.ToList();
        }
    }
}
