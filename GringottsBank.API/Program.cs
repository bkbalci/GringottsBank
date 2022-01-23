using GringottsBank.DAL.Abstract;
using GringottsBank.DAL.PostgreSQL.Contexts;
using GringottsBank.DAL.PostgreSQL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GringottsBank.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var appDbContext = services.GetRequiredService<BankContext>();
                    appDbContext.Database.Migrate();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var customerRepository = services.GetRequiredService<ICustomerRepository>();
                    var accountRepository = services.GetRequiredService<IAccountRepository>();
                    if (!userManager.Users.Any())
                    {
                        userManager.CreateAsync(new ApplicationUser { UserName = "apiuser", Email = "apiuser@gringotts.com" }, "Password12*").Wait();
                        customerRepository.AddAsync(new Entities.Concrete.Customer { Id = 1, IdentityNumber = "123", FirstName = "Burak Koray", LastName = "Balci", CreatedDate = DateTime.Now }).Wait();
                        customerRepository.AddAsync(new Entities.Concrete.Customer { Id = 2, IdentityNumber = "124", FirstName = "Test", LastName = "Customer", CreatedDate = DateTime.Now }).Wait();
                        accountRepository.AddAsync(new Entities.Concrete.Account { Id = 1, CustomerId = 1, Name = "BKB", Balance = 0, CreatedDate = DateTime.Now }).Wait();
                        accountRepository.AddAsync(new Entities.Concrete.Account { Id = 2, CustomerId = 1, Name = "BKB 2", Balance = 0, CreatedDate = DateTime.Now }).Wait();
                        accountRepository.AddAsync(new Entities.Concrete.Account { Id = 3, CustomerId = 2, Name = "TEST", Balance = 0, CreatedDate = DateTime.Now }).Wait();

                    }
                }
                logger.Info("API started");
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "API start failed");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
