using AutoMapper;
using GringottsBank.BLL.Abstract;
using GringottsBank.BLL.Extensions;
using GringottsBank.DAL.Abstract;
using GringottsBank.Entities.Concrete;
using GringottsBank.Entities.DTO.Shared;
using GringottsBank.Entities.DTO.Transaction;
using GringottsBank.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.BLL.Concrete
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<TransactionDto>>> GetListByAccountIdAsync(int accountId)
        {
            bool accountExists = await CustomerExistsCheck(accountId);
            if (!accountExists)
            {
                return ApiResponse<List<TransactionDto>>.Fail(404, "Account not found");
            }
            TransactionFilter filter = new TransactionFilter();
            filter.AccountId = accountId;
            var filterExpresion = filter.CreateFilterExpression<Transaction, TransactionFilter>();
            var transactions = await _transactionRepository.GetListAsync(filterExpresion);
            var transactionsDto = _mapper.Map<List<TransactionDto>>(transactions);
            return ApiResponse<List<TransactionDto>>.Success(200, transactionsDto);
        }


        public async Task<ApiResponse<List<TransactionDto>>> GetListByCustomerAsync(int customerId, DateTime startDate, DateTime endDate)
        {
            bool customerExists = await CustomerExistsCheck(customerId);
            if (!customerExists)
            {
                return ApiResponse<List<TransactionDto>>.Fail(404, "Customer not found");
            }
            TransactionFilter filter = new TransactionFilter();
            filter.CustomerId = customerId;
            filter.TransactionDateStart = startDate;
            filter.TransactionDateEnd = endDate;
            var filterExpresion = filter.CreateFilterExpression<Transaction, TransactionFilter>();
            var transactions = await _transactionRepository.GetListAsync(filterExpresion);
            var transactionsDto = _mapper.Map<List<TransactionDto>>(transactions);
            return ApiResponse<List<TransactionDto>>.Success(200, transactionsDto);
        }

        public async Task<ApiResponse<NoContent>> SaveAsync(TransactionSaveDto transactionSaveDto)
        {
            AccountFilter filter = new AccountFilter();
            filter.Id = transactionSaveDto.AccountId;
            var accountFilterExpression = filter.CreateFilterExpression<Account, AccountFilter>();
            var account = await _accountRepository.GetAsync(accountFilterExpression);
            if (account == null)
            {
                return ApiResponse<NoContent>.Fail(404, "Account not found.");
            }
            else if (account.Balance + transactionSaveDto.Amount < 0)
            {
                return ApiResponse<NoContent>.Fail(400, $"You cannot exceed account balance ({account.Balance}).");
            }
            var transaction = _mapper.Map<Transaction>(transactionSaveDto);
            transaction.TransactionDate = DateTime.Now;
            using (var dbTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _unitOfWork.TransactionRepository.BeginAdd(transaction);
                    account.Balance += transaction.Amount;
                    _unitOfWork.AccountRepository.BeginUpdate(account);
                    await _unitOfWork.SaveChangesAsync();
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    return ApiResponse<NoContent>.Fail(500, $"There was an error while saving transaction. Error : {ex.Message}");
                }
            }
            return ApiResponse<NoContent>.Success(200);
        }

        private async Task<bool> AccountExistsCheck(int accountId)
        {
            AccountFilter filter = new AccountFilter();
            filter.Id = accountId;
            var filterExpresion = filter.CreateFilterExpression<Account, AccountFilter>();
            bool customerExists = await _accountRepository.ExistAsync(filterExpresion);
            return customerExists;
        }

        private async Task<bool> CustomerExistsCheck(int customerId)
        {
            CustomerFilter filter = new CustomerFilter();
            filter.Id = customerId;
            var filterExpresion = filter.CreateFilterExpression<Customer, CustomerFilter>();
            bool customerExists = await _customerRepository.ExistAsync(filterExpresion);
            return customerExists;
        }
    }
}
