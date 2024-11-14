using loginAPP.Business;
using loginAPP.Context;
using loginAPP.Model;
using loginAPP.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace loginAPP.Controllers

{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {

        private readonly IExpense _expense;

        public ExpenseController(IExpense expense)
        {
            _expense = expense;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            var expense = _expense.GetExpensesAsync();
            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseVM model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized("UserId don't have token");
            }

            model.UserId = Guid.Parse(userIdClaim);

            var result = await _expense.AddExpenseAsync(model);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var result = await _expense.DeleteExpenseAsync(id);
            if (!result)
            {
                return NotFound("Expense not found.");
            }
            return Ok("Expense deleted successfully.");
        }


    }
}
