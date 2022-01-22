using GringottsBank.Entities.DTO.Account;
using GringottsBank.Entities.DTO.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.BLL.Abstract
{
    public interface IAccountService
    {
        Task<ApiResponse<AccountDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<AccountDto>>> GetListByCustomerIdAsync(int customerId);
        Task<ApiResponse<NoContent>> SaveAsync(AccountSaveDto accountSaveDto);
    }
}
