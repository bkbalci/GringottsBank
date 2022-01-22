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
    public class CustomerRepository : EntityRepositoryBase<Customer, BankContext>, ICustomerRepository
    {
        private readonly BankContext _dbContext;
        public CustomerRepository(BankContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
