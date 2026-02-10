using AcmeCorporation.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Core.Data;

public class DbContext : IdentityDbContext<User>
{
    public DbContext(DbContextOptions<DbContext> options) 
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
            
            entity.HasOne<User>()
                .WithMany(u => u.Submissions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SerialNumber>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Number).IsUnique();
        });
    }
}