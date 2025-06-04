using loginAPP.Business.UnitOfWork;
using loginAPP.ViewModel;
using MediatR;

namespace loginAPP.Business.CQRS.Queries
{
    public class GetExpensesQuery : IRequest<List<ExpenseVM>>
    {
    }

    public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, List<ExpenseVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetExpensesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ExpenseVM>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _unitOfWork.ExpenseRepository.GetAllAsync();
            
            return expenses.Select(e => new ExpenseVM
            {
                Id = e.Id,
                Date = e.Date,
                Description = e.Description,
                Amount = e.Amount,
                UserId = e.UserId,
                Type = e.Type
            }).ToList();
        }
    }
} 