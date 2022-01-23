using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace GringottsBank.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : CustomControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;
        string userName;

        public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _transactionService = transactionService;
            _logger = logger;
            userName = httpContextAccessor.HttpContext.User.Identity.Name;
        }

        [HttpGet("GetByCustomer")]
        public async Task<IActionResult> GetListByCustomerAsync([FromQuery] TransactionByCustomerDto request)
        {
            _logger.Log(LogLevel.Information, "Transactions with {customerId} customer id fetched by {userName}", request.CustomerId, userName);
            var apiResponse = await _transactionService.GetListByCustomerAsync(request.CustomerId, request.StartDate, request.EndDate);
            return CreateActionResult(apiResponse);
        }

        [HttpGet("GetByAccountId/{accountId}")]
        public async Task<IActionResult> GetListByAccountIdAsync(int accountId)
        {
            _logger.Log(LogLevel.Information, "Transactions with {accountId} account id fetched by {userName}", accountId, userName);
            var apiResponse = await _transactionService.GetListByAccountIdAsync(accountId);
            return CreateActionResult(apiResponse);
        }


        [HttpPost("Deposit")]
        public async Task<IActionResult> DepositAsync([FromBody] TransactionSaveDto transactionSaveDto)
        {
            _logger.Log(LogLevel.Information, "Deposit made by {userName}. Transaction Info -> {transactionInfo}", userName, JsonSerializer.Serialize(transactionSaveDto));
            var apiResponse = await _transactionService.SaveAsync(transactionSaveDto);
            return CreateActionResult(apiResponse);
        }

        [HttpPost("Withdraw")]
        public async Task<IActionResult> WithdrawAsync([FromBody] TransactionSaveDto transactionSaveDto)
        {
            _logger.Log(LogLevel.Information, "Withdraw made by {userName}. Transaction Info -> {transactionInfo}", userName, JsonSerializer.Serialize(transactionSaveDto));
            transactionSaveDto.Amount = -1 * transactionSaveDto.Amount;
            var apiResponse = await _transactionService.SaveAsync(transactionSaveDto);
            return CreateActionResult(apiResponse);
        }
    }
}
