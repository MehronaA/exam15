using System;
using Infrasrtucture.APIResponces;
using Infrastructure.DTOs.Account;

namespace Infrastructure.Interfaces;

public interface IAccountService
{
    Task<Responce<List<AccountGetDto>>> GetItemsAsync();
    Task<Responce<AccountGetDto>> GetItemById(int id);
    Task<Responce<string>> CreateItemAsync(AccountCreateDto dto);
    Task<Responce<string>> UpdateItemAsync(int id,AccountUpdateDto dto);
    Task<Responce<string>> DeleteItemAsync(int id);

}
