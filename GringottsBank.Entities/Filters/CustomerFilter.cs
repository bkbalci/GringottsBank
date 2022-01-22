using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.Entities.Filters
{
    public class CustomerFilter
    {
        public int? Id { get; set; }
        public string IdentityNumber { get; set; }
    }
}
