using GringottsBank.Entities.DTO.Customer;
using GringottsBank.Entities.DTO.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.BLL.Abstract
{
    public interface ICustomerService
    {
        Task<ApiResponse<CustomerDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<CustomerDto>>> GetListAsync();
        Task<ApiResponse<NoContent>> SaveAsync(CustomerSaveDto customerSaveDto);
    }
}
