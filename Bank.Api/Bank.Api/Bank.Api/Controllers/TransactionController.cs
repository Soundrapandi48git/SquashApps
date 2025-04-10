using AutoMapper;
using Domain.DataTransferObjects;
using Domain.Entities;
using Infrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Bank.Api.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        private ILogger _logger;
        public TransactionController(ILogger<TransactionController> logger, IRepositoryWrapper wrapper, IMapper mapper)
        {
            _repository = wrapper;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        [Route("getalltransactiondetails")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _repository.TransactionRepository.GetAllAsync();
                var gettransaction= _mapper.Map<IEnumerable<TransactionDto>>(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while fetching transaction details : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("getspecifictransactiondetails/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _repository.TransactionRepository.GetByIdAsync(id);
                var gettransaction = _mapper.Map<TransactionDto>(response);
                return Ok(gettransaction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while fetching transaction details : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        [Route("createtransactiondetails")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] TransactionDto dtorequest)
        {
            try
            {
                var transaction = _mapper.Map<TransactionInfo>(dtorequest);
                transaction.Timestamp = DateTime.UtcNow;
                var balance=await _repository.AccountRepository.GetByIdAsync(dtorequest.UserID);
                if (balance == null)
                {
                    return NotFound("Account not found");
                }
                await _repository.TransactionRepository.CreateAsync(transaction);
                var balanceAmount = dtorequest.TransactionType.ToLowerInvariant()=="credit"?balance.Balance+dtorequest.Amount:(balance.Balance - dtorequest.Amount);
                var account = new AccountDto()
                {
                    UserId = dtorequest.UserID,
                    Balance = balanceAmount,
                    AccountNumber=dtorequest.AccountNumber,

                };
                var accountMap = _mapper.Map<AccountUserInfo>(account);
                accountMap.UpdatedDate = DateTime.UtcNow;
                _repository.AccountRepository.UpdateAsync(accountMap);
                await _repository.SaveAsync();
                var response = _mapper.Map<TransactionDto>(transaction);    
                return Ok(new { Amount=response.Amount,Message=dtorequest.TransactionType.ToString().ToLowerInvariant()=="credit"?"Amount credited successfully":"Amount debited successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while fetching transaction details : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
