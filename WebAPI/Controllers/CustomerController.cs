using System;
using Infrasrtucture.APIResponces;
using Infrasrtucture.DTOs.Customer;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/customer")]
public class CustomerController
{
    private readonly ICustomerService customerService;
    public CustomerController(ICustomerService service)
    {
        customerService = service;
    }
    [HttpGet]
    public async Task<Responce<List<CustomerGetDto>>> GetItemsAsync()
    {
        return await customerService.GetItemsAsync();
    }
    [HttpGet("{id:int}")]

    public async Task<Responce<CustomerGetDto>> GetItemByIdAsync(int id)
    {
        return await customerService.GetItemByIdAsync(id);

    }
    [HttpPost]
    public async Task<Responce<string>> CreateItemAsync([FromForm]CustomerCreateDto dto)
    {
        return await customerService.CreateItemAsync(dto);

    }
    [HttpPut("id:int")]
    public async Task<Responce<string>> UpdateItemAsync(int id, [FromForm]CustomerUpdateDto dto)
    { 
        return await customerService.UpdateItemAsync(id,dto);
        
    }
    [HttpDelete("id:int")]
    
    public async Task<Responce<string>> DeleteItemAsync(int id)
    {
        return await customerService.DeleteItemAsync(id);
    } 
}
