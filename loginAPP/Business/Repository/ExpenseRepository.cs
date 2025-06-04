using loginAPP.Context;
using loginAPP.Model;
using Microsoft.EntityFrameworkCore;

namespace loginAPP.Business.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetAllAsync()
        {
            return await _context.Expenses.ToListAsync();
        }

        public async Task<List<Expense>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<Expense> AddAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            return expense;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await GetByIdAsync(id);
            if (expense == null)
                return false;

            _context.Expenses.Remove(expense);
            return true;
        }

        public async Task<Expense> GetByIdAsync(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }
    }
} 