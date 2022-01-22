using GringottsBank.DAL.Abstract;
using GringottsBank.DAL.GenericRepository;
using GringottsBank.DAL.PostgreSQL.Contexts;
using GringottsBank.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.DAL.PostgreSQL.Concrete
{
    public class AccountRepository : EntityRepositoryBase<Account, BankContext>, IAccountRepository
    {
        private readonly BankContext _dbContext;
        public AccountRepository(BankContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
