using System;
using Infrasrtucture.APIResponces;
using Infrastructure.DTOs.Account;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController
{
    private readonly IAccountService accountService;
    public AccountController(IAccountService service)
    {
        accountService = service;
    }
    [HttpGet]
    public async Task<Responce<List<AccountGetDto>>> GetItemsAsync()
    {
        return await accountService.GetItemsAsync();
    }
    [HttpGet("{id:int}")]
    
    public async Task<Responce<AccountGetDto>> GetItemById(int id)
    {
        return await accountService.GetItemById(id);

    }
    [HttpPost]

    public async Task<Responce<string>> CreateItemAsync(AccountCreateDto dto)
    {
        return await accountService.CreateItemAsync(dto);

    }
    [HttpPut("{id:int}")]
    public async Task<Responce<string>> UpdateItemAsync(int id, AccountUpdateDto dto)
    {
        return await accountService.UpdateItemAsync(id, dto);

    }
    [HttpDelete("{id:int}")]
    
    public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        return await accountService.DeleteItemAsync(id);

    }
}
