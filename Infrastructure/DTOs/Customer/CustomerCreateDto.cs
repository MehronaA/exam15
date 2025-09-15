using System;
using Microsoft.AspNetCore.Http;

namespace Infrasrtucture.DTOs.Customer;

public class CustomerCreateDto
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public IFormFile? ProfilePicture{ get; set; }
}

