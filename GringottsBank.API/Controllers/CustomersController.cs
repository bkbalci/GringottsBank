using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GringottsBank.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : CustomControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var apiResponse = await _customerService.GetByIdAsync(id);
            return CreateActionResult(apiResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var apiResponse = await _customerService.GetListAsync();
            return CreateActionResult(apiResponse);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(CustomerSaveDto customerSaveDto)
        {
            var apiResponse = await _customerService.SaveAsync(customerSaveDto);
            return CreateActionResult(apiResponse);
        }
    }
}
