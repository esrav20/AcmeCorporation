using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AcmeCorporation.Core.Models;

namespace AcmeCorporation.Web.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<DrawSubmission> Submissions => Set<DrawSubmission>();
    public DbSet<SerialNumber> SerialNumbers => Set<SerialNumber>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Identity-tables

        modelBuilder.Entity<DrawSubmission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email);

            // Relation: Submission → SerialNumber
            entity.HasOne(e => e.SerialNumber)
                .WithMany(s => s.Submissions)
                .HasForeignKey(e => e.SerialNumberId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation: Submission → User
            entity.HasOne(e => e.User)
                .WithMany(u => u.Submissions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

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