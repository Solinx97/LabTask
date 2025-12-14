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

    public DbSet<Link> Link { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>(builder =>
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.CreatedAt)
                .IsRequired();

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
            builder.HasKey(c => c.Id);

            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(Domain.Entities.Comment.CONTENT_MAX_LENGTH);

            builder.Property(c => c.DocumentId)
                .IsRequired();

            builder.Property(c => c.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<Link>(builder =>
        {
            builder.HasKey(l => l.Id);

            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.Property(l => l.Uri)
                .IsRequired();

            builder.Property(l => l.DocumentId)
                .IsRequired();

            builder.Property(l => l.OwnerId)
                .IsRequired();

            builder.Property(l => l.ToUserId)
                .IsRequired();

            builder.Property(l => l.ExpireAt)
                .IsRequired();
        });
    }
}