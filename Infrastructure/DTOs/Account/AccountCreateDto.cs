using System;

namespace Infrastructure.DTOs.Account;

public class AccountCreateDto
{
    public int CustomerId { get; set; }
    public Guid AccountNuber { get; set; } = Guid.NewGuid();
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
}
