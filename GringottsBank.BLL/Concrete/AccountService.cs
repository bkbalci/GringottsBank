using AutoMapper;
using GringottsBank.BLL.Abstract;
using GringottsBank.BLL.Extensions;
using GringottsBank.DAL.Abstract;
using GringottsBank.Entities.Concrete;
using GringottsBank.Entities.DTO.Account;
using GringottsBank.Entities.DTO.Shared;
using GringottsBank.Entities.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.BLL.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, ICustomerRepository customerRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AccountDto>> GetByIdAsync(int id)
        {
            AccountFilter filter = new AccountFilter();
            filter.Id = id;
            var filterExpresion = filter.CreateFilterExpression<Account, AccountFilter>();
            Func<IQueryable<Account>, IQueryable<Account>> includes = x => x.Include(y => y.Customer).Include(y => y.Transactions);
            var account = await _accountRepository.GetAsync(filterExpresion, includes);
            if (account == null)
            {
                return ApiResponse<AccountDto>.Fail(404, "Account not found.");
            }
            var accountDto = _mapper.Map<AccountDto>(account);
            return ApiResponse<AccountDto>.Success(200, accountDto);
        }

        public async Task<ApiResponse<List<AccountDto>>> GetListByCustomerIdAsync(int customerId)
        {
            bool customerExists = await CustomerExistsCheck(customerId);
            if (!customerExists)
            {
                return ApiResponse<List<AccountDto>>.Fail(404, "Customer not found.");
            }
            AccountFilter filter = new AccountFilter();
            filter.CustomerId = customerId;
            var filterExpresion = filter.CreateFilterExpression<Account, AccountFilter>();
            var accounts = await _accountRepository.GetListAsync(filterExpresion);
            var accountsDto = _mapper.Map<List<AccountDto>>(accounts);
            return ApiResponse<List<AccountDto>>.Success(200, accountsDto);
        }

        public async Task<ApiResponse<NoContent>> SaveAsync(AccountSaveDto accountSaveDto)
        {
            bool customerExists = await CustomerExistsCheck(accountSaveDto.CustomerId);
            if (!customerExists)
            {
                return ApiResponse<NoContent>.Fail(404, "Customer not found.");
            }
            var account = _mapper.Map<Account>(accountSaveDto);
            account.CreatedDate = DateTime.Now;
            await _accountRepository.AddAsync(account);
            return ApiResponse<NoContent>.Success(200);
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
