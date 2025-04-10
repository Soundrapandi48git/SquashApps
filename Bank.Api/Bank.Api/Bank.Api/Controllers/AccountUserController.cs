using AutoMapper;
using Domain;
using Domain.DataTransferObjects;
using Domain.Entities;
using Infrastructure.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Bank.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountUserController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        private ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly RepositoryDbContext _dbContext;
        public AccountUserController(ILogger<AccountUserController> logger, IRepositoryWrapper wrapper, IMapper mapper,
            IConfiguration configuration,RepositoryDbContext dbContext)
        {
            _configuration = configuration;
            _repository = wrapper;
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]

        [Route("getallaccountdetails")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var resposne = await _repository.AccountRepository.GetAllAsync();
                var getAllAccounts = _mapper.Map<IEnumerable<AccountDto>>(resposne);
                return Ok(getAllAccounts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while fetching account details : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet]
        [Route("getspecificaccountdetails/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var resposne = await _repository.AccountRepository.GetByIdAsync(id);
                var getAccout = _mapper.Map<AccountDto>(resposne);
                return Ok(getAccout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while fetching account details {id} : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpPost]
        [Route("createaccountdetails")]
        public async Task<IActionResult> Post([FromBody] AccountDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var account = _mapper.Map<AccountUserInfo>(request);
                account.AccountNumber = "SBI"+DateTime.UtcNow.ToString("yyyyMMddhhmmssfff");
                account.CreatedBy = "User";
                account.CreatedDate = DateTime.UtcNow;
                account.RegisteredOn = DateTime.UtcNow;
                await _repository.AccountRepository.CreateAsync(account);
                await _repository.SaveAsync();
                var accountData = _mapper.Map<AccountDto>(account);
                var token = GenerateToken(request.Username);
                return Ok(new {token});
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while Creating Account : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPut]
        [Route("updateaccountdetails/{id}")]
        public IActionResult Put(int id, [FromBody] AccountDto dtoRequest)
        {
            try
            {
                var account = _repository.AccountRepository.GetByIdAsync(id).GetAwaiter().GetResult();
                if (account == null)
                {
                    return BadRequest("Account informatioin not found, please contact with administrator for furrether assitance");
                }
                _mapper.Map(dtoRequest, account);
                _repository.AccountRepository.UpdateAsync(account);
                _repository.SaveAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while updating  Account details : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            return Ok();
        }
        [HttpDelete]
        [Route("deleteaccountdetails/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var account = _repository.AccountRepository.GetByIdAsync(id).GetAwaiter().GetResult();
                if (account == null)
                {
                    return BadRequest("Account informatioin not found, please contact with administrator for furrether assitance");
                }
                _repository.AccountRepository.DeleteAsync(account);
                _repository.SaveAsync().GetAwaiter().GetResult();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while deleting the account details : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] Loginrequest login)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var logindetails = _dbContext.Accountinformation.FirstOrDefault(x => x.Email == login.Email);
            if (logindetails != null)
            {
                var token = GenerateToken(logindetails.Username);
                return Ok(new { token });
            }
            return Unauthorized("Invalid credentials.");
        }
        private string GenerateToken(string username)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            string issuer = jwtSettings["Issuer"];
            string audience = jwtSettings["Audience"];
            string secrtekey = jwtSettings["Key"];
            var claims = new[]
           {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrtekey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(60)),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
