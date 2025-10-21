namespace Valoriza.Domain.Entities;


public enum Category { Essentials, Leisure, Gambling, Investment, Other }


public class TransactionRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }


    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public Category Category { get; set; } = Category.Other;
    public DateTime Date { get; set; } = DateTime.UtcNow;


    public bool IsRisky { get; set; }
}