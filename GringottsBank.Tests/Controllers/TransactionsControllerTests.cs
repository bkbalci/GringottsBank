using GringottsBank.API.Controllers;
using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Shared;
using GringottsBank.Entities.DTO.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GringottsBank.Tests.Controllers
{
    public class TransactionsControllerTests
    {
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly Mock<ILogger<TransactionsController>> _mockLogger;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly TransactionsController _controller;
        Random rand;

        public TransactionsControllerTests()
        {
            _mockTransactionService = new Mock<ITransactionService>();
            _mockLogger = new Mock<ILogger<TransactionsController>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContextAccessor.Setup(_ => _.HttpContext.User.Identity.Name).Returns("test");
            _controller = new TransactionsController(_mockTransactionService.Object, _mockLogger.Object, _mockHttpContextAccessor.Object);
            rand = new Random();
        }

        [Fact]
        public async Task GetListByAccountIdAsync_ReturnsOk()
        {
            _mockTransactionService.Setup(x => x.GetListByAccountIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ApiResponse<List<TransactionDto>>.Success(200, new List<TransactionDto> { CreateRandomTransaction(2), CreateRandomTransaction(2), CreateRandomTransaction(2) }));
            var response = (ObjectResult)await _controller.GetListByAccountIdAsync(5);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task GetListByAccountIdAsync_ReturnsNotFound()
        {
            _mockTransactionService.Setup(x => x.GetListByAccountIdAsync(It.IsAny<int>()));
            var response = (ObjectResult)await _controller.GetListByAccountIdAsync(5);
            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task GetListByAccountIdAsync_ReturnsItems()
        {
            var transactions = new List<TransactionDto> { CreateRandomTransaction(2), CreateRandomTransaction(2), CreateRandomTransaction(2) };
            _mockTransactionService.Setup(x => x.GetListByAccountIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ApiResponse<List<TransactionDto>>.Success(200, transactions));
            var response = (ObjectResult)await _controller.GetListByAccountIdAsync(5);
            Assert.Equal((response.Value as ApiResponse<List<TransactionDto>>).Data, transactions);
        }

        [Fact]
        public async Task GetListByCustomerAsync_ReturnsOk()
        {
            _mockTransactionService.Setup(x => x.GetListByCustomerAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(ApiResponse<List<TransactionDto>>.Success(200));
            var response = (ObjectResult)await _controller.GetListByCustomerAsync(new TransactionByCustomerDto());
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task GetListByCustomerAsync_ReturnsNotFound()
        {
            _mockTransactionService.Setup(x => x.GetListByCustomerAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            var response = (ObjectResult)await _controller.GetListByCustomerAsync(new TransactionByCustomerDto());
            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task GetListByCustomerAsync_ReturnsItems()
        {
            _mockTransactionService.Setup(x => x.GetListByCustomerAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(ApiResponse<List<TransactionDto>>.Success(200, new List<TransactionDto> { CreateRandomTransaction(2), CreateRandomTransaction(2), CreateRandomTransaction(2) }));
            var response = (ObjectResult)await _controller.GetListByCustomerAsync(new TransactionByCustomerDto());
            Assert.NotEmpty((response.Value as ApiResponse<List<TransactionDto>>).Data);
        }

        [Fact]
        public async Task Deposit_ReturnsOk()
        {
            _mockTransactionService.Setup(x => x.SaveAsync(It.IsAny<TransactionSaveDto>()))
                .ReturnsAsync(ApiResponse<NoContent>.Success(200));
            var response = (ObjectResult)await _controller.DepositAsync(new TransactionSaveDto());
            Assert.Equal(200, response.StatusCode);
        }
        [Fact]
        public async Task Deposit_ReturnsNotFound()
        {
            _mockTransactionService.Setup(x => x.SaveAsync(It.IsAny<TransactionSaveDto>()));
            var response = (ObjectResult)await _controller.DepositAsync(new TransactionSaveDto());
            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task Withdraw_ReturnsOk()
        {
            _mockTransactionService.Setup(x => x.SaveAsync(It.IsAny<TransactionSaveDto>()))
                .ReturnsAsync(ApiResponse<NoContent>.Success(200));
            var response = (ObjectResult)await _controller.WithdrawAsync(new TransactionSaveDto());
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task Withdraw_ReturnsNotFound()
        {
            _mockTransactionService.Setup(x => x.SaveAsync(It.IsAny<TransactionSaveDto>()));
            var response = (ObjectResult)await _controller.WithdrawAsync(new TransactionSaveDto());
            Assert.Equal(404, response.StatusCode);
        }

        private TransactionDto CreateRandomTransaction(int accountId = 0)
        {
            return new TransactionDto
            {
                Id = rand.Next(),
                AccountId = accountId > 0 ? accountId : rand.Next(),
                Amount = rand.Next(),
            };
        }
    }
}
