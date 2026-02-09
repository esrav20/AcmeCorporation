using Microsoft.EntityFrameworkCore;
using AcmeCorporation.Core.Models;

namespace AcmeCorporation.Core.Data;

public class AcmeDbContext : DbContext
{
    public AcmeDbContext(DbContextOptions<AcmeDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Submission> Submissions { get; set; } = null!;
    public DbSet<SerialNumber> SerialNumbers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.SerialNumber);
        });

        modelBuilder.Entity<SerialNumber>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SN).IsUnique();
        });
    }
}