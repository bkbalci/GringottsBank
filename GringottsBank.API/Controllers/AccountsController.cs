using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Account;
using GringottsBank.Entities.DTO.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Threading.Tasks;

namespace GringottsBank.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController : CustomControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountsController> _logger;
        string userName;

        public AccountsController(IAccountService accountService, ILogger<AccountsController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _logger = logger;
            userName = httpContextAccessor.HttpContext.User.Identity.Name;
        }

        /// <summary>
        /// Get account.
        /// </summary>
        /// <returns>Account info</returns>
        /// <remarks>
        /// 
        /// Get an account from bank.
        /// 
        /// Sample requests:
        /// 
        /// GET /api/accounts/1
        /// 
        /// </remarks>
        /// <response code="200">Returns account object</response>
        /// <response code="404">Account doesn't exists</response>
        /// <response code="500">There is an error in the system</response>
        [ProducesResponseType(typeof(ApiResponse<AccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AccountDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.Log(LogLevel.Information, "Account with {id} id fetched by {userName}", id, userName);
            var apiResponse = await _accountService.GetByIdAsync(id);
            return CreateActionResult(apiResponse);
        }

        /// <summary>
        /// Get account list of a customer.
        /// </summary>
        /// <returns>Account list</returns>
        /// <remarks>
        /// 
        /// Get account list from bank by customer id.
        /// 
        /// Sample requests:
        /// 
        /// GET /api/accounts/GetByCustomerId/1
        /// 
        /// </remarks>
        /// <response code="200">Returns account list</response>
        /// <response code="404">Customer doesn't exists</response>
        /// <response code="500">There is an error in the system</response>
        [ProducesResponseType(typeof(ApiResponse<List<AccountDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<AccountDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status500InternalServerError)]
        [HttpGet("GetByCustomerId/{customerId}")]
        public async Task<IActionResult> GetListByCustomerIdAsync(int customerId)
        {
            _logger.Log(LogLevel.Information, "Account with {customerId} customer id fetched by {userName}", customerId, userName);
            var apiResponse = await _accountService.GetListByCustomerIdAsync(customerId);
            return CreateActionResult(apiResponse);
        }

        /// <summary>
        /// Save an account.
        /// </summary>
        /// <returns>Status code</returns>
        /// <remarks>
        /// 
        /// Save an account of customer into the bank.
        /// 
        /// </remarks>
        /// <response code="200">Account added</response>
        /// <response code="400">There is an error in the request</response>
        /// <response code="404">Customer doesn't exists</response>
        /// <response code="500">There is an error in the system</response>
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(AccountSaveDto accountSaveDto)
        {
            _logger.Log(LogLevel.Information, "An account saved by {userName}. Account info -> {accountInfo}", userName, JsonSerializer.Serialize(accountSaveDto));
            var apiResponse = await _accountService.SaveAsync(accountSaveDto);
            return CreateActionResult(apiResponse);
        }
    }
}
