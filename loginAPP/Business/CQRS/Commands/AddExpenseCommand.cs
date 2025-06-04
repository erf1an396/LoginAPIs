using loginAPP.Business.UnitOfWork;
using loginAPP.Model;
using loginAPP.ViewModel;
using MediatR;

namespace loginAPP.Business.CQRS.Commands
{
    public class AddExpenseCommand : IRequest<ExpenseVM>
    {
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
    }

    public class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommand, ExpenseVM>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddExpenseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ExpenseVM> Handle(AddExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = new Expense
            {
                UserId = request.UserId,
                Description = request.Description,
                Date = request.Date,
                Amount = request.Amount,
                Type = request.Type
            };

            var result = await _unitOfWork.ExpenseRepository.AddAsync(expense);
            await _unitOfWork.SaveChangesAsync();

            return new ExpenseVM
            {
                Id = result.Id,
                UserId = result.UserId,
                Description = result.Description,
                Date = result.Date,
                Amount = result.Amount,
                Type = result.Type
            };
        }
    }
} 