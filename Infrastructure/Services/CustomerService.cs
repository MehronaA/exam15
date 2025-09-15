using System;
using Dapper;
using Infrasrtucture.DTOs.Customer;
using Infrasrtucture.APIResponces;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly DataContext _context;
    private readonly IFileService _fileService;


    public CustomerService(DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Responce<string>> CreateItemAsync(CustomerCreateDto dto)
    {

        try
        {
         var fileName = await _fileService.SaveFileAsync("Customer", dto.ProfilePicture!);
        await using var connection = _context.GetConnection();
        connection.Open();
        var checkingEmail = "select * from customers where email=@email";
        var resultEmail = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(checkingEmail, new { Email = dto.Email });
        if (resultEmail != null) return Responce<string>.Fail(404, "Customer with this email already exist");
        var cmd = "insert into customers(name,email,phoneNumber,address,profilePicture) values(@name,@email,@phoneNumber,@address,@profilePicture)";
        var result = await connection.ExecuteAsync(cmd, new
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            ProfilePicture = dto.ProfilePicture
        });
        return result == 0
                ? Responce<string>.Fail(500, "Internal Server Error")
                : Responce<string>.Created("Customer successfully created");   
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
        var cmd1 = "select * from customers where customerId =@id ";
        var res1 = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(cmd1, new { id });
        if (res1 == null) return Responce<string>.Fail(404, "Customer with you account number doesnt exist");
        var cmd = "delete from customers where customerId = @id";
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

    public async Task<Responce<CustomerGetDto>> GetItemByIdAsync(int id)
    {
        try
        {
            await using var connection = _context.GetConnection();
        connection.Open();
        var cmd = "select * from customers where customerId=@id";
        var result = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(cmd, new { id });
        return result == null
                ? Responce<CustomerGetDto>.Fail(404, "Not found")
                : Responce<CustomerGetDto>.Ok(result, null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<Responce<List<CustomerGetDto>>> GetItemsAsync()
    {
        try
        {
            await using var connection = _context.GetConnection();
        connection.Open();
        var cmd = "select * from customers";
        var result = await connection.QueryAsync<CustomerGetDto>(cmd);
        return Responce<List<CustomerGetDto>>.Ok(result.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        

    }

    public async Task<Responce<string>> UpdateItemAsync(int id, CustomerUpdateDto dto)
    {
        try
        {
            await using var connection = _context.GetConnection();
        connection.Open();

        var cmd1 = "select * from customers where customerId = @id";
        var customer = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(cmd1, new { id });
        if (customer == null)
        {
            return Responce<string>.Fail(404, "Customer not found");
        }

        var fileName = customer.ProfilePicture;
        if (dto.ProfilePicture != null)
        {
            _fileService.DeleteFileAsync("Customer", customer.ProfilePicture);
            fileName = await _fileService.SaveFileAsync("Customers", dto.ProfilePicture);
        }

        var cmd = $"update customers set name=@name, email=@email,phoneNumber=@phoneNumber,address=@address, profilePicture=@profilePicture where customerId = @id";

        var result = await connection.ExecuteAsync(cmd,
            new
            {
                Id = id,
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                ProfilePicture = dto.ProfilePicture
            });

        return result == 0
            ? Responce<string>.Fail(500, "Internal Server Error")
            : Responce<string>.Created("Customer  successfully updated");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}
