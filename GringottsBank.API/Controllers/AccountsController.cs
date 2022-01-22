using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GringottsBank.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : CustomControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var apiResponse = await _accountService.GetByIdAsync(id);
            return CreateActionResult(apiResponse);
        }

        [HttpGet("GetByCustomerId/{customerId}")]
        public async Task<IActionResult> GetListByCustomerIdAsync(int customerId)
        {
            var apiResponse = await _accountService.GetListByCustomerIdAsync(customerId);
            return CreateActionResult(apiResponse);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(AccountSaveDto accountSaveDto)
        {
            var apiResponse = await _accountService.SaveAsync(accountSaveDto);
            return CreateActionResult(apiResponse);
        }
    }
}
