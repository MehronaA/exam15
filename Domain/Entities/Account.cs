using System;

namespace Domain.Entities;

public class Account
{
    public int AccountId { get; set; }
    public int AccountNuber { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; }

}
