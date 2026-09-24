using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Url> Urls => Set<Url>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Url>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.OriginalUrl)
                .IsRequired();

            entity.Property(x => x.ShortCode)
                .IsRequired()
                .HasMaxLength(10);

            entity.HasIndex(x => x.ShortCode)
                .IsUnique();

            entity.Property(x => x.ClickCount)
                .HasDefaultValue(0);

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}