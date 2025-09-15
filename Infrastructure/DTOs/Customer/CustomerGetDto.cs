using System;
namespace Infrasrtucture.DTOs.Customer;

public class CustomerGetDto
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string ProfilePicture { get; set; }   
}
