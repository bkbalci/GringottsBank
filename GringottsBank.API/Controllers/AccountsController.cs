using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Threading.Tasks;

namespace GringottsBank.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
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


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.Log(LogLevel.Information, "Account with {id} id fetched by {userName}", id, userName);
            var apiResponse = await _accountService.GetByIdAsync(id);
            return CreateActionResult(apiResponse);
        }

        [HttpGet("GetByCustomerId/{customerId}")]
        public async Task<IActionResult> GetListByCustomerIdAsync(int customerId)
        {
            _logger.Log(LogLevel.Information, "Account with {customerId} customer id fetched by {userName}", customerId, userName);
            var apiResponse = await _accountService.GetListByCustomerIdAsync(customerId);
            return CreateActionResult(apiResponse);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(AccountSaveDto accountSaveDto)
        {
            _logger.Log(LogLevel.Information, "An account saved by {userName}. Account info -> {accountInfo}", userName, JsonSerializer.Serialize(accountSaveDto));
            var apiResponse = await _accountService.SaveAsync(accountSaveDto);
            return CreateActionResult(apiResponse);
        }
    }
}
