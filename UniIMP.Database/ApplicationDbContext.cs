using Microsoft.EntityFrameworkCore;
using UniIMP.DataAccess.Entities;

namespace UniIMP.DataAccess
{
#pragma warning disable 8618

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetTag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AssetTag>(e =>
            {
                e.ToTable(name: "Tags");
                e.HasIndex(x => x.Name).IsUnique();

                e.HasMany(at => at.Assets)
                 .WithMany(a => a.Tags)
                 .UsingEntity(x =>
                 {
                     x.ToTable("AssetTags");
                 });
            });

            // Asset Entity Confituration
            builder.Entity<Asset>(e =>
            {
                e.ToTable(name: "Assets");

                e.HasOne(sa => sa.Agent)
                 .WithOne(a => a.Asset)
                 .IsRequired(false);
            });

            builder.Entity<SnmpAgent>(e =>
            {
                e.ToTable(name: "SnmpAgents");

                e.HasOne(sa => sa.Asset)
                 .WithOne(a => a.Agent)
                 .IsRequired(true);
            });
        }
    }

#pragma warning restore 8618
}