using System;

namespace Infrastructure.DTOs.Account;

public class AccountCreateDto
{
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
}
