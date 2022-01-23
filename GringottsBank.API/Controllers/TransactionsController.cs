using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Shared;
using GringottsBank.Entities.DTO.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace GringottsBank.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
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

        /// <summary>
        /// Get transaction list of a customer.
        /// </summary>
        /// <returns>Transaction list</returns>
        /// <remarks>
        /// 
        /// Get transaction list from bank by customer id and date range.
        /// 
        /// 
        /// </remarks>
        /// <response code="200">Returns transaction list</response>
        /// <response code="404">Customer doesn't exists</response>
        /// <response code="500">There is an error in the system</response>
        [ProducesResponseType(typeof(ApiResponse<List<TransactionDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TransactionDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status500InternalServerError)]
        [HttpGet("GetByCustomer")]
        public async Task<IActionResult> GetListByCustomerAsync([FromQuery] TransactionByCustomerDto request)
        {
            _logger.Log(LogLevel.Information, "Transactions with {customerId} customer id fetched by {userName}", request.CustomerId, userName);
            var apiResponse = await _transactionService.GetListByCustomerAsync(request.CustomerId, request.StartDate, request.EndDate);
            return CreateActionResult(apiResponse);
        }

        /// <summary>
        /// Get transaction list of an account.
        /// </summary>
        /// <returns>Transaction list</returns>
        /// <remarks>
        /// 
        /// Get transaction list from bank by account id.
        /// 
        /// 
        /// </remarks>
        /// <response code="200">Returns transaction list</response>
        /// <response code="404">Account doesn't exists</response>
        /// <response code="500">There is an error in the system</response>
        [ProducesResponseType(typeof(ApiResponse<List<TransactionDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TransactionDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status500InternalServerError)]
        [HttpGet("GetByAccountId/{accountId}")]
        public async Task<IActionResult> GetListByAccountIdAsync(int accountId)
        {
            _logger.Log(LogLevel.Information, "Transactions with {accountId} account id fetched by {userName}", accountId, userName);
            var apiResponse = await _transactionService.GetListByAccountIdAsync(accountId);
            return CreateActionResult(apiResponse);
        }


        /// <summary>
        /// Deposit money to an account.
        /// </summary>
        /// <returns>Status code</returns>
        /// <remarks>
        /// 
        /// Deposit money to an account.
        /// 
        /// </remarks>
        /// <response code="200">Succesful</response>
        /// <response code="400">There is an error in the request</response>
        /// <response code="404">Account doesn't exists</response>
        /// <response code="500">There is an error in the system</response>
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status500InternalServerError)]
        [HttpPost("Deposit")]
        public async Task<IActionResult> DepositAsync([FromBody] TransactionSaveDto transactionSaveDto)
        {
            _logger.Log(LogLevel.Information, "Deposit made by {userName}. Transaction Info -> {transactionInfo}", userName, JsonSerializer.Serialize(transactionSaveDto));
            var apiResponse = await _transactionService.SaveAsync(transactionSaveDto);
            return CreateActionResult(apiResponse);
        }

        /// <summary>
        /// Withdraw money from an account.
        /// </summary>
        /// <returns>Status code</returns>
        /// <remarks>
        /// 
        /// Withdraw money from an account.
        /// 
        /// </remarks>
        /// <response code="200">Succesful</response>
        /// <response code="400">There is an error in the request</response>
        /// <response code="404">Account doesn't exists</response>
        /// <response code="500">There is an error in the system</response>
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status500InternalServerError)]
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
