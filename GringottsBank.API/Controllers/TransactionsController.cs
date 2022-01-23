using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GringottsBank.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : CustomControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("GetByCustomer")]
        public async Task<IActionResult> GetListByCustomerAsync([FromQuery] TransactionByCustomerDto request)
        {
            var apiResponse = await _transactionService.GetListByCustomerAsync(request.CustomerId, request.StartDate, request.EndDate);
            return CreateActionResult(apiResponse);
        }

        [HttpGet("GetByAccountId/{accountId}")]
        public async Task<IActionResult> GetListByAccountIdAsync(int accountId)
        {
            var apiResponse = await _transactionService.GetListByAccountIdAsync(accountId);
            return CreateActionResult(apiResponse);
        }


        [HttpPost("Deposit")]
        public async Task<IActionResult> DepositAsync([FromBody] TransactionSaveDto transactionSaveDto)
        {
            var apiResponse = await _transactionService.SaveAsync(transactionSaveDto);
            return CreateActionResult(apiResponse);
        }

        [HttpPost("Withdraw")]
        public async Task<IActionResult> WithdrawAsync([FromBody] TransactionSaveDto transactionSaveDto)
        {
            transactionSaveDto.Amount = -1 * transactionSaveDto.Amount;
            var apiResponse = await _transactionService.SaveAsync(transactionSaveDto);
            return CreateActionResult(apiResponse);
        }
    }
}
