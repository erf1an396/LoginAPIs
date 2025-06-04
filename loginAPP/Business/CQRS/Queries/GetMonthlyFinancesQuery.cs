using loginAPP.Business.UnitOfWork;
using loginAPP.ViewModel;
using MediatR;

namespace loginAPP.Business.CQRS.Queries
{
    public class GetMonthlyFinancesQuery : IRequest<List<MontlyFinanceVM>>
    {
        public string UserId { get; set; }
    }

    public class GetMonthlyFinancesQueryHandler : IRequestHandler<GetMonthlyFinancesQuery, List<MontlyFinanceVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMonthlyFinancesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MontlyFinanceVM>> Handle(GetMonthlyFinancesQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _unitOfWork.ExpenseRepository.GetByUserIdAsync(new Guid(request.UserId));
            
            return expenses
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .Select(g => new MontlyFinanceVM
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Where(e => e.Type == "واریز").Sum(e => e.Amount),
                    TotalExpense = g.Where(e => e.Type == "برداشت").Sum(e => e.Amount)
                }).ToList();
        }
    }
} 