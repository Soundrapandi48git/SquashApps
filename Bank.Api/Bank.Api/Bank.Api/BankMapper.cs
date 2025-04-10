using AutoMapper;
using Domain.DataTransferObjects;
using Domain.Entities;
using System.Runtime;

namespace Bank.Api
{
    public class BankMapper: Profile
    {
        public BankMapper()
        {
            CreateMap<AccountUserInfo, AccountDto>().ReverseMap();
            CreateMap<TransactionInfo, TransactionDto>().ReverseMap();
        }
    }
}
