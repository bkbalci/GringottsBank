using AutoMapper;
using GringottsBank.BLL.Abstract;
using GringottsBank.BLL.Extensions;
using GringottsBank.DAL.Abstract;
using GringottsBank.Entities.Concrete;
using GringottsBank.Entities.DTO.Customer;
using GringottsBank.Entities.DTO.Shared;
using GringottsBank.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.BLL.Concrete
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CustomerDto>> GetByIdAsync(int id)
        {
            CustomerFilter filter = new CustomerFilter();
            filter.Id = id;
            var filterExpresion = filter.CreateFilterExpression<Customer, CustomerFilter>();
            var customer = await _customerRepository.GetAsync(filterExpresion);
            if (customer == null)
            {
                return ApiResponse<CustomerDto>.Fail(404, "Customer not found.");
            }
            var customerDto = _mapper.Map<CustomerDto>(customer);
            return ApiResponse<CustomerDto>.Success(200, customerDto);
        }

        public async Task<ApiResponse<List<CustomerDto>>> GetListAsync()
        {
            var customers = await _customerRepository.GetListAsync();
            var customersDto = _mapper.Map<List<CustomerDto>>(customers);
            return ApiResponse<List<CustomerDto>>.Success(200, customersDto);
        }

        public async Task<ApiResponse<NoContent>> SaveAsync(CustomerSaveDto customerSaveDto)
        {
            CustomerFilter filter = new CustomerFilter();
            filter.IdentityNumber = customerSaveDto.IdentityNumber;
            var filterExpresion = filter.CreateFilterExpression<Customer, CustomerFilter>();
            bool customerExists = await _customerRepository.ExistAsync(filterExpresion);
            if (customerExists)
            {
                return ApiResponse<NoContent>.Fail(400, "Customer already exists.");
            }
            var customer = _mapper.Map<Customer>(customerSaveDto);
            customer.CreatedDate = DateTime.Now;
            await _customerRepository.AddAsync(customer);
            return ApiResponse<NoContent>.Success(200);
        }

    }
}
