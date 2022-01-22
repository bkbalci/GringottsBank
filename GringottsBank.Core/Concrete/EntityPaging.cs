using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.Core.Concrete
{
    public class EntityPaging
    {
        public int? PageNumber { get; set; }
        public int? ItemCount { get; set; }
    }
}
