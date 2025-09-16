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
            if (string.IsNullOrWhiteSpace(dto.Name))        return Responce<string>.Fail(400, "Name is required");
            if (string.IsNullOrWhiteSpace(dto.Address))     return Responce<string>.Fail(400, "Address is required");
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber)) return Responce<string>.Fail(400, "Phone is required");
            if (string.IsNullOrWhiteSpace(dto.Email))       return Responce<string>.Fail(400, "Email is required");
            //check name
            if (dto.Name.Trim().Length < 2 || dto.Name.Trim().Length > 50) return Responce<string>.Fail(422, "Unprocessable Entity");

            //address check
            if (dto.Address.Trim().Length < 10 || dto.Address.Trim().Length > 150) return Responce<string>.Fail(422, "Unprocessable Entity");

            //phone length check
            if (dto.PhoneNumber.Trim().Length<7 || dto.PhoneNumber.Trim().Length>11 ) return Responce<string>.Fail(422, "Unprocessable Entity");

            var fileName = await _fileService.SaveFileAsync("Customer", dto.ProfilePicture);

            await using var connection = _context.GetConnection();
            connection.Open();

            //emailcheck
            var checkingEmail = "select * from customers where email=@email";
            var resultEmail = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(checkingEmail, new { Email = dto.Email });
            if (resultEmail != null) return Responce<string>.Fail(404, "Customer with this email already exist");

            //chech phone
            var phoneCheck = "select * from customers where phoneNumber=@phoneNumber";
            var phoneResult = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(phoneCheck, new { PhoneNumber = dto.PhoneNumber });
            if (phoneResult != null) return Responce<string>.Fail(409, "Customer with this phone number exist");

            //main
            var cmd = "insert into customers(name,email,phoneNumber,address,profilePicture) values(@name,@email,@phoneNumber,@address,@profilePicture)";
            var result = await connection.ExecuteAsync(cmd, new
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                ProfilePicture = fileName
            });
            return result == 0
                ? Responce<string>.Fail(500, "Internal Server Error")
                : Responce<string>.Created("Customer successfully created");   
        }
        catch (Exception e)
        {
            _fileService.DeleteFileAsync("Customer", dto.ProfilePicture.FileName);
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
            var cmd1 = "select * from customers where customerId =@id ";
            var res1 = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(cmd1, new { id });
            if (res1 == null) return Responce<string>.Fail(404, "Customer with you account number doesnt exist");

            //main
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
            if (string.IsNullOrWhiteSpace(dto.Name))    return Responce<string>.Fail(400, "Name is required");
            if (string.IsNullOrWhiteSpace(dto.Address)) return Responce<string>.Fail(400, "Address is required");
            if (string.IsNullOrWhiteSpace(dto.Email))   return Responce<string>.Fail(400, "Email is required");
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber)) return Responce<string>.Fail(400, "Phone is required");
            //check name
            if (dto.Name.Trim().Length < 2 || dto.Name.Trim().Length > 50) Responce<string>.Fail(422, "Unprocessable Entity");

            //address check
            if (dto.Address.Trim().Length < 10 || dto.Address.Trim().Length > 150) return Responce<string>.Fail(422, "Unprocessable Entity");

            await using var connection = _context.GetConnection();
            connection.Open();

            //check if exist
            var cmd1 = "select * from customers where customerId = @id";
            var customer = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(cmd1, new { id });
            if (customer == null) return Responce<string>.Fail(404, "Customer not found");
            
            //emailcheck
            var checkingEmail = "select * from customers where email=@email";
            var resultEmail = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(checkingEmail, new { Email = dto.Email });
            if (resultEmail != null) return Responce<string>.Fail(404, "Customer with this email already exist");

            //chech phone
            var phoneCheck = "select * from customers where phoneNumber=@phoneNumber";
            var phoneResult = await connection.QueryFirstOrDefaultAsync<CustomerGetDto>(phoneCheck, new { PhoneNumber = dto.PhoneNumber });
            if (phoneResult != null) return Responce<string>.Fail(409, "Customer with this phone number exist");

            var fileName = customer.ProfilePicture;

            
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
                ProfilePicture = fileName
            });

            return result == 0
            ? Responce<string>.Fail(500, "Internal Server Error")
            : Responce<string>.Created("Customer  successfully updated");
        }
        catch (Exception e)
        {
            _fileService.DeleteFileAsync("Customer", dto.ProfilePicture.FileName);
            Console.WriteLine(e);
            throw;
        }
        
    }
}
