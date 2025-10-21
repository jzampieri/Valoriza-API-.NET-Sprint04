﻿namespace Valoriza.Domain.Entities;


public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public ICollection<TransactionRecord> Transactions { get; set; } = new List<TransactionRecord>();
}