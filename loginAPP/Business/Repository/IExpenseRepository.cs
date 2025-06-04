using loginAPP.Model;

namespace loginAPP.Business.Repository
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetAllAsync();
        Task<List<Expense>> GetByUserIdAsync(Guid userId);
        Task<Expense> AddAsync(Expense expense);
        Task<bool> DeleteAsync(int id);
        Task<Expense> GetByIdAsync(int id);
    }
} 