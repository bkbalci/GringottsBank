using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Customer;
using GringottsBank.Entities.DTO.Shared;
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

        /// <summary>
        /// Get customer.
        /// </summary>
        /// <returns>Customer info</returns>
        /// <remarks>
        /// 
        /// Get a customer from bank.
        /// 
        /// Sample requests:
        /// 
        /// GET /api/customers/1
        /// 
        /// </remarks>
        /// <response code="200">Returns customer object</response>
        /// <response code="404">Customer doesn't exists</response>
        /// <response code="500">There is an error in the system</response>
        [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.Log(LogLevel.Information, "Customer with {id} id fetched by {userName}", id, userName);
            var apiResponse = await _customerService.GetByIdAsync(id);
            return CreateActionResult(apiResponse);
        }

        /// <summary>
        /// Get customer list.
        /// </summary>
        /// <returns>Customer list</returns>
        /// <remarks>
        /// 
        /// Get customer list from bank.
        /// 
        /// Sample requests:
        /// 
        /// GET /api/customers
        /// 
        /// </remarks>
        /// <response code="200">Returns customer list</response>
        /// <response code="500">There is an error in the system</response>
        [ProducesResponseType(typeof(ApiResponse<List<CustomerDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            _logger.Log(LogLevel.Information, "Customer list fetched by {userName}", userName);
            var apiResponse = await _customerService.GetListAsync();
            return CreateActionResult(apiResponse);
        }


        /// <summary>
        /// Save a customer.
        /// </summary>
        /// <returns>Status code</returns>
        /// <remarks>
        /// 
        /// Save customer into the bank.
        /// 
        /// </remarks>
        /// <response code="200">Customer added</response>
        /// <response code="400">There is an error in the request</response>
        /// <response code="500">There is an error in the system</response>
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<NoContent>), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(CustomerSaveDto customerSaveDto)
        {
            _logger.Log(LogLevel.Information, "A customer saved by {userName}. Customer info -> {customerInfo}", userName, JsonSerializer.Serialize(customerSaveDto));
            var apiResponse = await _customerService.SaveAsync(customerSaveDto);
            return CreateActionResult(apiResponse);
        }
    }
}
