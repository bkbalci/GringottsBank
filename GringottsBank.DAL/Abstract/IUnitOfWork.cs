using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsBank.DAL.Abstract
{
    public interface IUnitOfWork
    {
        public IAccountRepository AccountRepository { get; }
        public ITransactionRepository TransactionRepository { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
