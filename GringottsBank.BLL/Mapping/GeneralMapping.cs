using AutoMapper;
using GringottsBank.Entities.Concrete;
using GringottsBank.Entities.DTO.Account;
using GringottsBank.Entities.DTO.Customer;
using GringottsBank.Entities.DTO.Transaction;

namespace GringottsBank.BLL.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Account, AccountSaveDto>().ReverseMap();

            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CustomerSaveDto>().ReverseMap();

            CreateMap<Transaction, TransactionBaseDto>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<Transaction, TransactionSaveDto>().ReverseMap();
        }
    }
}
