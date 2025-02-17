using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.AppDbContext
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<OrderEntity> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the Order entity to the "Orders" container and define the partition key.
            modelBuilder.Entity<OrderEntity>()
                .ToContainer("Orders")
                .HasPartitionKey(o => o.PartitionKey)
                // Optionally, you can specify a discriminator if using inheritance.
                .HasNoDiscriminator();

            // Optionally, configure the property mapping for OrderId to Cosmos DB's id.
            modelBuilder.Entity<OrderEntity>().Property(o => o.OrderNo).ToJsonProperty("id");

            base.OnModelCreating(modelBuilder);
        }
    }
}
