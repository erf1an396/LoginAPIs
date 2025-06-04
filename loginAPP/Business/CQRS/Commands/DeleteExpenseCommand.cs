using loginAPP.Business.UnitOfWork;
using MediatR;

namespace loginAPP.Business.CQRS.Commands
{
    public class DeleteExpenseCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteExpenseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ExpenseRepository.DeleteAsync(request.Id);
            if (!result)
                return false;

            return await _unitOfWork.SaveChangesAsync();
        }
    }
} 