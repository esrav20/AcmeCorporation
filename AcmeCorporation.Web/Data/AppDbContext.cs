using AcmeCorporation.Core.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Web.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    // Dependency Injection of DbContextOptions
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Create database tables for Submission and SerialNumber
    public DbSet<DrawSubmission> Submissions => Set<DrawSubmission>();
    public DbSet<SerialNumber> SerialNumbers => Set<SerialNumber>();

    // Create relations for Submission and SerialNumber
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Identity-tables

        
        // Submissions has key, index and relation to SerialNumber
        modelBuilder.Entity<DrawSubmission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email);
            // Relation: Submission → SerialNumber
            entity.HasOne(e => e.SerialNumber)
                .WithMany(s => s.Submissions)
                .HasForeignKey(e => e.SerialNumberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        // SerialNumbers has key and index
        modelBuilder.Entity<SerialNumber>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Number).IsUnique();
        });
    }

    // Auto-set SubmittedAt
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<DrawSubmission>())
        {
            if (entry.State == EntityState.Added && entry.Entity.SubmittedAt == default)
                entry.Entity.SubmittedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}