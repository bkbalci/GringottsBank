using GringottsBank.API.Controllers;
using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Account;
using GringottsBank.Entities.DTO.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GringottsBank.Tests.Controllers
{
    public class AccountsControllerTests
    {
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly Mock<ILogger<AccountsController>> _mockLogger;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly AccountsController _controller;
        Random rand;

        public AccountsControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _mockLogger = new Mock<ILogger<AccountsController>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContextAccessor.Setup(_ => _.HttpContext.User.Identity.Name).Returns("test");
            _controller = new AccountsController(_mockAccountService.Object, _mockLogger.Object, _mockHttpContextAccessor.Object);
            rand = new Random();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOk()
        {
            _mockAccountService.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ApiResponse<AccountDto>.Success(200, CreateRandomAccount()));
            var response = (ObjectResult) await _controller.GetByIdAsync(5);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound()
        {
            _mockAccountService.Setup(x => x.GetByIdAsync(It.IsAny<int>()));
            var response = (ObjectResult)await _controller.GetByIdAsync(5);
            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsItem()
        {
            var account = CreateRandomAccount();
            _mockAccountService.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ApiResponse<AccountDto>.Success(200, account));
            var response = (ObjectResult)await _controller.GetByIdAsync(5);
            Assert.Equal((response.Value as ApiResponse<AccountDto>).Data, account);
        }

        [Fact]
        public async Task GetListByCustomerIdAsync_ReturnsOk()
        {
            _mockAccountService.Setup(x => x.GetListByCustomerIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ApiResponse<List<AccountDto>>.Success(200));
            var response = (ObjectResult)await _controller.GetListByCustomerIdAsync(5);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task GetListByCustomerIdAsync_ReturnsNotFound()
        {
            _mockAccountService.Setup(x => x.GetListByCustomerIdAsync(It.IsAny<int>()));
            var response = (ObjectResult)await _controller.GetListByCustomerIdAsync(5);
            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task GetListByCustomerIdAsync_ReturnsItems()
        {
            _mockAccountService.Setup(x => x.GetListByCustomerIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ApiResponse<List<AccountDto>>.Success(200, new List<AccountDto> { CreateRandomAccount(2), CreateRandomAccount(2), CreateRandomAccount(2) }));
            var response = (ObjectResult)await _controller.GetListByCustomerIdAsync(5);
            Assert.NotEmpty((response.Value as ApiResponse<List<AccountDto>>).Data);
        }

        [Fact]
        public async Task SaveAsync_ReturnsOk()
        {
            _mockAccountService.Setup(x => x.SaveAsync(It.IsAny<AccountSaveDto>()))
                .ReturnsAsync(ApiResponse<NoContent>.Success(200));
            var response = (ObjectResult)await _controller.SaveAsync(new AccountSaveDto());
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task SaveAsync_ReturnsNotFound()
        {
            _mockAccountService.Setup(x => x.SaveAsync(It.IsAny<AccountSaveDto>()));
            var response = (ObjectResult)await _controller.SaveAsync(new AccountSaveDto ());
            Assert.Equal(404, response.StatusCode);
        }

        private AccountDto CreateRandomAccount(int customerId = 0)
        {
            return new AccountDto
            {
                Id = rand.Next(),
                Balance = (decimal)rand.NextDouble(),
                CustomerId = customerId == 0 ? rand.Next() : customerId,
            };
        }
    }
}
