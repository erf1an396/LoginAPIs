using loginAPP.Business.Repository;

namespace loginAPP.Business.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IExpenseRepository ExpenseRepository { get; }
        Task<bool> SaveChangesAsync();
    }
} 