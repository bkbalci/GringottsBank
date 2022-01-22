using GringottsBank.BLL.Abstract;
using GringottsBank.BLL.Concrete;
using GringottsBank.DAL.Abstract;
using GringottsBank.DAL.PostgreSQL.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace GringottsBank.BLL.Extensions
{
    public static class DependencyResolvers
    {
        public static void BindDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ITransactionService, TransactionService>();
        }
    }
}
