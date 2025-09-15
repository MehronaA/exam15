using System;
using Infrasrtucture.APIResponces;
using Infrasrtucture.DTOs.Customer;

namespace Infrastructure.Interfaces;

public interface ICustomerService
{
    Task<Responce<List<CustomerGetDto>>> GetItemsAsync();
    Task<Responce<CustomerGetDto>> GetItemByIdAsync(int id);
    Task<Responce<string>> CreateItemAsync(CustomerCreateDto dto);
    Task<Responce<string>> UpdateItemAsync(int id,CustomerUpdateDto dto);
    Task<Responce<string>> DeleteItemAsync(int id); 
}
