using GringottsBank.Entities.DTO.Customer;
using GringottsBank.Entities.DTO.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.Entities.DTO.Account
{
    public class AccountDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }

        public CustomerDto Customer { get; set; }
        public List<TransactionBaseDto> Transactions { get; set; }
    }
}
