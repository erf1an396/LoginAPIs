using loginAPP.Business.UnitOfWork;
using loginAPP.ViewModel;
using MediatR;

namespace loginAPP.Business.CQRS.Queries
{
    public class GetExpensesByUserIdQuery : IRequest<List<ExpenseVM>>
    {
        public string UserId { get; set; }
    }

    public class GetExpensesByUserIdQueryHandler : IRequestHandler<GetExpensesByUserIdQuery, List<ExpenseVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetExpensesByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ExpenseVM>> Handle(GetExpensesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _unitOfWork.ExpenseRepository.GetByUserIdAsync(new Guid(request.UserId));
            
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