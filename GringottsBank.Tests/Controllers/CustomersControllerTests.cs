using GringottsBank.API.Controllers;
using GringottsBank.BLL.Abstract;
using GringottsBank.Entities.DTO.Customer;
using GringottsBank.Entities.DTO.Shared;
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
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly Mock<ILogger<CustomersController>> _mockLogger;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly CustomersController _controller;
        Random rand;

        public CustomersControllerTests()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _mockLogger = new Mock<ILogger<CustomersController>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContextAccessor.Setup(_ => _.HttpContext.User.Identity.Name).Returns("test");
            _controller = new CustomersController(_mockCustomerService.Object, _mockLogger.Object, _mockHttpContextAccessor.Object);
            rand = new Random();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOk()
        {
            _mockCustomerService.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ApiResponse<CustomerDto>.Success(200, CreateRandomAccount()));
            var response = (ObjectResult)await _controller.GetByIdAsync(5);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound()
        {
            _mockCustomerService.Setup(x => x.GetByIdAsync(It.IsAny<int>()));
            var response = (ObjectResult)await _controller.GetByIdAsync(5);
            Assert.Equal(404, response.StatusCode);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsItem()
        {
            var customer = CreateRandomAccount();
            _mockCustomerService.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ApiResponse<CustomerDto>.Success(200, customer));
            var response = (ObjectResult)await _controller.GetByIdAsync(5);
            Assert.Equal((response.Value as ApiResponse<CustomerDto>).Data, customer);
        }

        [Fact]
        public async Task GetListByCustomerIdAsync_ReturnsOk()
        {
            _mockCustomerService.Setup(x => x.GetListAsync())
                .ReturnsAsync(ApiResponse<List<CustomerDto>>.Success(200));
            var response = (ObjectResult)await _controller.GetListAsync();
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task GetListByCustomerIdAsync_ReturnsItems()
        {
            _mockCustomerService.Setup(x => x.GetListAsync())
                .ReturnsAsync(ApiResponse<List<CustomerDto>>.Success(200, new List<CustomerDto> { CreateRandomAccount(), CreateRandomAccount(), CreateRandomAccount() }));
            var response = (ObjectResult)await _controller.GetListAsync();
            Assert.NotEmpty((response.Value as ApiResponse<List<CustomerDto>>).Data);
        }

        [Fact]
        public async Task SaveAsync_ReturnsOk()
        {
            _mockCustomerService.Setup(x => x.SaveAsync(It.IsAny<CustomerSaveDto>()))
                .ReturnsAsync(ApiResponse<NoContent>.Success(200));
            var response = (ObjectResult)await _controller.SaveAsync(new CustomerSaveDto());
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task SaveAsync_ReturnsNotFound()
        {
            _mockCustomerService.Setup(x => x.SaveAsync(It.IsAny<CustomerSaveDto>()));
            var response = (ObjectResult)await _controller.SaveAsync(new CustomerSaveDto());
            Assert.Equal(404, response.StatusCode);
        }

        private CustomerDto CreateRandomAccount()
        {
            return new CustomerDto
            {
                FirstName = "",
                LastName = "",
                IdentityNumber = ""
            };
        }
    }
}
