using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Customer;
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
    public class CustomersController : CustomControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;
        string userName;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _customerService = customerService;
            _logger = logger;
            userName = httpContextAccessor.HttpContext.User.Identity.Name;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.Log(LogLevel.Information, "Customer with {id} id fetched by {userName}", id, userName);
            var apiResponse = await _customerService.GetByIdAsync(id);
            return CreateActionResult(apiResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            _logger.Log(LogLevel.Information, "Customer list fetched by {userName}", userName);
            var apiResponse = await _customerService.GetListAsync();
            return CreateActionResult(apiResponse);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(CustomerSaveDto customerSaveDto)
        {
            _logger.Log(LogLevel.Information, "A customer saved by {userName}. Customer info -> {customerInfo}", userName, JsonSerializer.Serialize(customerSaveDto));
            var apiResponse = await _customerService.SaveAsync(customerSaveDto);
            return CreateActionResult(apiResponse);
        }
    }
}
