using System;
using System.Diagnostics;
using Dapper;
using Infrasrtucture.APIResponces;
using Infrasrtucture.DTOs.Customer;
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
            //balance
            if (dto.Balance < 0) return Responce<string>.Fail(400, "Balance cannot be negative");
            await using var connection = _context.GetConnection();
            connection.Open();

            //check accountType
            if (dto.AccountType.Trim() != "savings" || dto.AccountType.Trim() != "checkings") return Responce<string>.Fail(409, "Wrong Account type");

            //account id
            var customerCheck = "select * from accounts where customerId=@id";
            var customerCheckResult = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(customerCheck, new { Id = dto.CustomerId });
            if (customerCheckResult != null) return Responce<string>.Fail(409, "This customer already has account ");

            //main
            var cmd = "insert into accounts(customerId,accountType,balance) values(@customerId,@accountType,@balance)";
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

            //check if exist
            var cmd1 = "select * from accounts where accountId =@id ";
            var res1 = await connection.QueryFirstOrDefaultAsync<AccountGetDto>(cmd1, new { id });
            if (res1 == null) return Responce<string>.Fail(404, $"Acoount to delete with your accountId '{id}' doesnt exist");

            //main
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
            //balance
            if (dto.Balance < 0) return Responce<string>.Fail(400, "Balance cannot be negative");

            await using var connection = _context.GetConnection();
            connection.Open();


            //check if exist
            var customerCheck = "select * from accounts where customerId=@id";
            var customerCheckResult = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(customerCheck, new { Id = dto.CustomerId });
            if (customerCheckResult != null) return Responce<string>.Fail(409, "This customer already has account ");

            //check id
            var cmd1 = "select * from accounts where acocountId=@id ";
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
