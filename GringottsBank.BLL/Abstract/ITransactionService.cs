using GringottsBank.Entities.DTO.Shared;
using GringottsBank.Entities.DTO.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.BLL.Abstract
{
    public interface ITransactionService
    {
        Task<ApiResponse<List<TransactionDto>>> GetListByAccountIdAsync(int accountId);
        Task<ApiResponse<List<TransactionDto>>> GetListByCustomerAsync(int customerId, DateTime startDate, DateTime endDate);
        Task<ApiResponse<NoContent>> SaveAsync(TransactionSaveDto transactionSaveDto);
    }
}
