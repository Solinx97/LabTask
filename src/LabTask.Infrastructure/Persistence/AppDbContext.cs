using LabTask.Domain.Aggregates;
using LabTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabTask.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Document> Document { get; set; } = null!;

    public DbSet<Comment> Comment { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>(builder =>
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(Domain.Aggregates.Document.NAME_MAX_LENGTH);

            builder.Property(d => d.Description)
                .IsRequired();

            builder.Property(d => d.ExpireAt)
                .IsRequired();

            builder.HasMany(c => c.Comments)
               .WithOne(m => m.Document)
               .HasForeignKey(m => m.DocumentId)
               .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(builder =>
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Content)
                .IsRequired()
                .HasMaxLength(Domain.Entities.Comment.CONTENT_MAX_LENGTH);
        });
    }
}