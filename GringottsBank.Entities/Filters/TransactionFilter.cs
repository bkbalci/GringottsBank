using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.Entities.Filters
{
    public class TransactionFilter
    {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        [DisplayName("Account.CustomerId")]
        public int? CustomerId { get; set; }
        public DateTime? TransactionDateStart { get; set; }
        public DateTime? TransactionDateEnd { get; set; }
    }
}
