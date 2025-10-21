using Microsoft.EntityFrameworkCore;
using Valoriza.Domain.Entities;


namespace Valoriza.Infrastructure;


public class ValorizaDbContext : DbContext
{
    public ValorizaDbContext(DbContextOptions<ValorizaDbContext> options) : base(options) { }


    public DbSet<User> Users => Set<User>();
    public DbSet<TransactionRecord> Transactions => Set<TransactionRecord>();
    public DbSet<RiskSignal> RiskSignals => Set<RiskSignal>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Name).HasMaxLength(120);
        });


        modelBuilder.Entity<TransactionRecord>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);


            e.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        });


        modelBuilder.Entity<RiskSignal>(e =>
        {
            e.HasKey(x => x.Id);
        });
    }
}