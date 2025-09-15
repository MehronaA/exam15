using System;
namespace Infrastructure.DTOs.Account;


public class AccountGetDto
{
    public int AccountNuber { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
}
