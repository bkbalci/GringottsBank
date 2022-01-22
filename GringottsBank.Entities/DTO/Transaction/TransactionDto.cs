using GringottsBank.Entities.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.Entities.DTO.Transaction
{
    public class TransactionDto : TransactionBaseDto
    {
        public AccountDto Account { get; set; }
    }
}
