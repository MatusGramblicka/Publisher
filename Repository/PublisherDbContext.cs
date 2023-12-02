using Contracts.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class PublisherDbContext : DbContext
{
    public PublisherDbContext(DbContextOptions<PublisherDbContext> options) : base(options)
    {
    }

    public DbSet<Author> Author { get; set; }
    public DbSet<Article> Article { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>()
            .HasKey(k => k.Id);
        modelBuilder.Entity<Author>()
            .HasIndex(i => i.Name)
            .IsUnique();
        modelBuilder.Entity<Author>()
            .HasOne(s => s.Image)
            .WithOne()
            .HasForeignKey<Image>("AuthorId")
            .IsRequired();

        modelBuilder.Entity<Article>()
            .HasKey(k => k.Id);
        modelBuilder.Entity<Article>()
            .HasIndex(i => i.Title);
        modelBuilder.Entity<Article>()
            .HasOne(e => e.Site)
            .WithMany()
            .IsRequired();
        modelBuilder.Entity<Article>()
            .HasMany(h => h.Author)
            .WithMany();

        modelBuilder.Entity<Site>()
            .HasKey(k => k.Id);

        modelBuilder.Entity<Image>()
            .HasKey(k => k.Id);
    }
}
