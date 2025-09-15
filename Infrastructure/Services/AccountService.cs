using System;
using Dapper;
using Infrasrtucture.APIResponces;
using Infrastructure.Data;
using Infrastructure.DTOs.Account;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class AccountService : IAccountService
{ 
    private readonly DataContext _context;
    public AccountService(DataContext context)
    {
        _context = context;
    }
    public async Task<Responce<string>> CreateItemAsync(AccountCreateDto dto)
    {
        try
        {
        await using var connection = _context.GetConnection();
        connection.Open();
        if (dto.Balance < 0) return Responce<string>.Fail(404, "Balance cannot be negative");
        var cmd = "insert into accounts(accountType,balance) values(@accountType,@balance)";
        var result = await connection.ExecuteAsync(cmd, dto);
        return result == 0
                ? Responce<string>.Fail(500, "Something goes wrong")
                : Responce<string>.Created("Created successfuly");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

       public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        try
        {
        await using var connection = _context.GetConnection();
        connection.Open();
        var cmd1 = "select * from accounts where accountNumber =@id ";
        var res1 = await connection.QueryFirstOrDefaultAsync<AccountGetDto>(cmd1, new { id });
        if (res1 == null) return Responce<string>.Fail(404, "Acoount with you account number doesnt exist");
        var cmd = "delete from accounts where accountNumber = @id";
        var result = await connection.ExecuteAsync(cmd, new { id });
        return result == 0
                ? Responce<string>.Fail(500, "Not deleted")
                : Responce<string>.Created("Deleted successfuly");   
        }
       catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<Responce<AccountGetDto>> GetItemById(int id)
    {
        try
        {
        await using var connection = _context.GetConnection();
        connection.Open();
        var cmd = "select * from accounts where accountNumber=@id";
        var result = await connection.QueryFirstOrDefaultAsync<AccountGetDto>(cmd, new { id});
        return result == null
                ? Responce<AccountGetDto>.Fail(404, "Not found")
                : Responce<AccountGetDto>.Ok(result, null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    public async Task<Responce<List<AccountGetDto>>> GetItemsAsync()
    {
        try
        {
        await using var connection = _context.GetConnection();
        connection.Open();
        var cmd = "select * from accounts";
        var result = await connection.QueryAsync<AccountGetDto>(cmd);
        return Responce<List<AccountGetDto>>.Ok(result.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    public async Task<Responce<string>> UpdateItemAsync(int id, AccountUpdateDto dto)
    {
        try
        {
            await using var connection = _context.GetConnection();
        connection.Open();
        if (dto.Balance < 0) return Responce<string>.Fail(404, "Balance cannot be negative");
        var cmd1 = "select * from account where accountNumber=@id ";
        var res1 = await connection.QueryFirstOrDefaultAsync<AccountGetDto>(cmd1, new { id });
        if (res1 == null) return Responce<string>.Fail(404, "account to update doesnt exist");
        var cmd = "update accounts set accountType=@accountType,balance=@balance where accountNumber=@id";
        var result = await connection.ExecuteAsync(cmd, new
        {
            Id = id,
            AccountType = dto.AccountType,
            Balance = dto.Balance
        });
        return result == 0
                ? Responce<string>.Fail(500, "Not updated")
                : Responce<string>.Created("Updated successfuly");
        }
       catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
         
    }

    
}
