using AutoMapper;
using Bank.Api.Controllers;
using Domain;
using Domain.DataTransferObjects;
using Domain.Entities;
using Infrastructure.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankTest
{
    public class AccountUserControllerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<ILogger<AccountUserController>> _mockLogger = new();
        private readonly Mock<IConfiguration> _config = new();
        private readonly Mock<RepositoryDbContext> _dbcontext = new();
        //private AccountUserController CreateController() { 
        //    new AccountUserController(_mockLogger.Object, _mockRepo.Object, _mockMapper.Object, _config.Object, _dbcontext.Object)


        //    }

        private AccountUserController CreateController()
        {
            return new AccountUserController(_mockLogger.Object, _mockRepo.Object, _mockMapper.Object, _config.Object, _dbcontext.Object);
        }
        [Fact]
        public async Task GetAllAccountDetails_ReturnsOkResult()
        {
            var controller = CreateController();
            _mockRepo.Setup(r => r.AccountRepository.GetAllAsync())
                .ReturnsAsync(new List<AccountUserInfo>());
            _mockMapper.Setup(m => m.Map<IEnumerable<AccountDto>>(It.IsAny<IEnumerable<AccountUserInfo>>()))
                .Returns(new List<AccountDto>());
            var result = await controller.Get();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<AccountDto>>(okResult.Value);
        }

        [Fact]
        public async Task GetAllAccountDetails_ThrowsException_Returns500()
        {
            var controller = CreateController();
            _mockRepo.Setup(r => r.AccountRepository.GetAllAsync())
                .ThrowsAsync(new Exception("DB Error"));

            var result = await controller.Get();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetSpecificAccountDetails_ValidId_ReturnsOk()
        {
            var controller = CreateController();
            _mockRepo.Setup(r => r.AccountRepository.GetByIdAsync(1))
                .ReturnsAsync(new AccountUserInfo());
            _mockMapper.Setup(m => m.Map<AccountDto>(It.IsAny<AccountUserInfo>()))
                .Returns(new AccountDto());

            var result = await controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AccountDto>(okResult.Value);
        }

        [Fact]
        public async Task CreateAccountDetails_ValidInput_ReturnsOk()
        {
            var controller = CreateController();
            var dto = new AccountDto { Username = "test", AccountNumber = "123" ,PhoneNumber="9632154870",Email="Test@gmail.com"};

            _mockMapper.Setup(m => m.Map<AccountUserInfo>(It.IsAny<AccountDto>()))
                .Returns(new AccountUserInfo());
            _mockRepo.Setup(r => r.AccountRepository.CreateAsync(It.IsAny<AccountUserInfo>()));
            _mockRepo.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<AccountDto>(It.IsAny<AccountUserInfo>()))
                .Returns(dto);

            var result = await controller.Post(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("AccountNumber", okResult.Value.ToString());
        }

        [Fact]
        public void UpdateAccountDetails_AccountNotFound_ReturnsBadRequest()
        {
            var controller = CreateController();
            var dto = new AccountDto { Username = "Newupdates", AccountNumber = "123", PhoneNumber = "9632154870", Email = "Test@gmail.com" };
            _mockRepo.Setup(r => r.AccountRepository.GetByIdAsync(1)).ReturnsAsync((AccountUserInfo)null);

            var result = controller.Put(1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Account informatioin not found, please contact with administrator for furrether assitance", badRequest.Value);
        }

        [Fact]
        public void DeleteAccountDetails_ValidId_ReturnsOk()
        {
            var controller = CreateController();
            _mockRepo.Setup(r => r.AccountRepository.GetByIdAsync(1)).ReturnsAsync(new AccountUserInfo());
            _mockRepo.Setup(r => r.AccountRepository.DeleteAsync(It.IsAny<AccountUserInfo>()));
            _mockRepo.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);
            var result = controller.Delete(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteAccountDetails_AccountNotFound_ReturnsBadRequest()
        {
            var controller = CreateController();
            _mockRepo.Setup(r => r.AccountRepository.GetByIdAsync(1)).ReturnsAsync((AccountUserInfo)null);
            var result = controller.Delete(1);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Account informatioin not found, please contact with administrator for furrether assitance", badRequest.Value);
        }
    }
}
