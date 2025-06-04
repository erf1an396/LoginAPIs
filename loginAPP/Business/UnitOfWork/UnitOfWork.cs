using loginAPP.Business.Repository;
using loginAPP.Context;

namespace loginAPP.Business.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IExpenseRepository _expenseRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IExpenseRepository ExpenseRepository
        {
            get { return _expenseRepository ??= new ExpenseRepository(_context); }
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
} 