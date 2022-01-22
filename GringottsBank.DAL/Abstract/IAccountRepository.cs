using GringottsBank.Core.Abstract;
using GringottsBank.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.DAL.Abstract
{
    public interface IAccountRepository : IEntityRepository<Account>
    {
    }
}
