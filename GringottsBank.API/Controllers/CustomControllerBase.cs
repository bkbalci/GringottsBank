using GringottsBank.Entities.DTO.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GringottsBank.API.Controllers
{
    public class CustomControllerBase
    {
        public IActionResult CreateActionResult<T>(ApiResponse<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode,
            };
        }
    }
}
