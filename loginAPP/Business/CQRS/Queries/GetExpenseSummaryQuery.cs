using loginAPP.Business.UnitOfWork;
using loginAPP.ViewModel;
using MediatR;

namespace loginAPP.Business.CQRS.Queries
{
    public class GetExpenseSummaryQuery : IRequest<ExpenseSummaryVM>
    {
        public string UserId { get; set; }
    }

    public class GetExpenseSummaryQueryHandler : IRequestHandler<GetExpenseSummaryQuery, ExpenseSummaryVM>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetExpenseSummaryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ExpenseSummaryVM> Handle(GetExpenseSummaryQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _unitOfWork.ExpenseRepository.GetByUserIdAsync(new Guid(request.UserId));
            
            var expenseVMs = expenses.Select(e => new ExpenseVM
            {
                Id = e.Id,
                Date = e.Date,
                Description = e.Description,
                Amount = e.Amount,
                UserId = e.UserId,
                Type = e.Type
            }).ToList();

            decimal totalIncome = expenses.Where(e => e.Type == "واریز").Sum(e => e.Amount);
            decimal totalExpense = expenses.Where(e => e.Type == "برداشت").Sum(e => e.Amount);
            decimal balance = totalIncome - totalExpense;

            return new ExpenseSummaryVM
            {
                Transactions = expenseVMs,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = balance
            };
        }
    }
} 