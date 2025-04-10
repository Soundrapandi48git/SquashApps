using AutoMapper;
using Bank.Api.Controllers;
using Domain.DataTransferObjects;
using Domain.Entities;
using Infrastructure.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankTest
{
    public class TransactionControllerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<TransactionController>> _mockLogger;
        private readonly TransactionController _controller;
        public TransactionControllerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<TransactionController>>();
            _controller = new TransactionController(_mockLogger.Object, _mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithAllTransactions()
        {
            var transactions = new List<TransactionInfo> { new TransactionInfo { AccountNumber = "1", Amount = 100 } };
            var mappedDtos = new List<TransactionDto> { new TransactionDto { AccountNumber = "1", Amount = 100 } };
            _mockRepo.Setup(repo => repo.TransactionRepository.GetAllAsync()).ReturnsAsync(transactions);
            _mockMapper.Setup(m => m.Map<IEnumerable<TransactionDto>>(transactions)).Returns(mappedDtos);
            var result = await _controller.Get();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(transactions, okResult.Value);
        }

        [Fact]
        public async Task Get_ById_ReturnsOkResult()
        {
            var transaction = new TransactionInfo { AccountNumber = "1", Amount = 200 };
            _mockRepo.Setup(repo => repo.TransactionRepository.GetByIdAsync(1)).ReturnsAsync(transaction);
            _mockMapper.Setup(m => m.Map<IEnumerable<TransactionDto>>(It.IsAny<TransactionInfo>())).Returns(new List<TransactionDto>());
            var result = await _controller.Get(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(transaction, okResult.Value);
        }

        [Fact]
        public async Task Post_ReturnsOkResult_WithCorrectMessage()
        {
            var dto = new TransactionDto { Amount = 300, TransactionType = "Credit" };
            var entity = new TransactionInfo { AccountNumber = "1", Amount = 300 };
            _mockMapper.Setup(m => m.Map<TransactionInfo>(dto)).Returns(entity);
            _mockRepo.Setup(r => r.TransactionRepository.CreateAsync(entity)).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<TransactionDto>(entity)).Returns(dto);
            var result = await _controller.Post(dto);
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic value = okResult.Value;
            Assert.Equal(300, value.Amount);
            Assert.Equal("Amount credited successfully", value.Message);
        }
    }
}
