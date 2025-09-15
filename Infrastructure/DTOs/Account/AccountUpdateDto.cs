using System;

namespace Infrastructure.DTOs.Account;


public class AccountUpdateDto
{
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
}
