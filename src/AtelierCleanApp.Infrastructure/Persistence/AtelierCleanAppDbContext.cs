using AtelierCleanApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AtelierCleanApp.Infrastructure.Persistence;

/// <summary>
/// Represents the database context for the AtelierCleanApp application.
/// It is the main class that coordinates Entity Framework functionality for a given data model.
/// </summary>
public class AtelierCleanAppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AtelierCleanAppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public AtelierCleanAppDbContext(DbContextOptions<AtelierCleanAppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the DbSet for Players.
    /// This property will be used to query and save instances of Player.
    /// EF Core will map this to a table named "Players" by convention.
    /// </summary>
    public DbSet<Player> Players { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for Countries.
    /// </summary>
    public DbSet<Country> Countries { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for PlayerData.
    /// </summary>
    public DbSet<PlayerData> PlayerData { get; set; }

    /// <summary>
    /// Configures the schema needed for the context.
    /// </summary>
    /// <param name="modelBuilder">
    /// The builder being used to construct the model for this context.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Country config
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(c => c.Code);

            entity.Property(c => c.Picture)
                .HasMaxLength(255)
                .IsRequired();
        });

        // Player config
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.ShortName)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(p => p.Sex)
                .HasMaxLength(1)
                .IsRequired();

            entity.Property(p => p.Picture)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasOne(p => p.Country)
                .WithMany(c => c.Players)
                .HasForeignKey(p => p.CountryCode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Data)
                .WithOne(d => d.Player)
                .HasForeignKey<PlayerData>(d => d.PlayerId);
        });

        // PlayerData config
        modelBuilder.Entity<PlayerData>(entity =>
        {
            entity.HasKey(d => d.PlayerId); // 1-to-1 with Player

            entity.Property(d => d.Rank)
                .IsRequired();

            entity.Property(d => d.Points)
                .IsRequired();

            entity.Property(d => d.Weight)
                .IsRequired();

            entity.Property(d => d.Height)
                .IsRequired();

            entity.Property(d => d.Age)
                .IsRequired();
            
            entity.Property(p => p.LastResults)
                .HasConversion(
                v => string.Join(',', v),           
                v => v.Split(',', StringSplitOptions.None).ToList());
        });
    }
}
