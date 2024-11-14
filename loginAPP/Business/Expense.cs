using loginAPP.Context;
using loginAPP.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using loginAPP.Model;

namespace loginAPP.Business
{

    public interface IExpense
    {
        Task<List<ExpenseVM>> GetExpensesAsync();

        Task<ExpenseVM> AddExpenseAsync(ExpenseVM model);

        Task<bool> DeleteExpenseAsync(int id);
    }

    public class Expense
    {
        private readonly AppDbContext _db;

        public Expense(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ExpenseVM>> GetExpensesAsync()
        {
           var resutl =  await _db.Expenses.Select(c => new ExpenseVM()
           {
               
               Date = c.Date,
               Description = c.Description,
               Amount = c.Amount,
               UserId = c.UserId,

           }).ToListAsync();



            return resutl;

        }

        public async Task<ExpenseVM> AddExpenseAsync(ExpenseVM model)
        {
            

            var expense = new Model.Expense()
            {
                UserId = model.UserId,
                Description = model.Description,
                Date = model.Date,
                Amount = model.Amount,
            };

            _db.Expenses.Add(expense);
            await _db.SaveChangesAsync();

            

            var expenseVM = new ExpenseVM()
            {
                UserId = expense.UserId,
                Description = expense.Description,
                Date = expense.Date,
                Amount = expense.Amount,
            };
            return expenseVM;

        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _db.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            if (expense != null)
                return false;

            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
