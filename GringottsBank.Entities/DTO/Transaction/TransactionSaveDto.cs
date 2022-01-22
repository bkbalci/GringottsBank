using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.Entities.DTO.Transaction
{
    public class TransactionSaveDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int AccountId { get; set; }
        [Required]
        [Range(0.1, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
