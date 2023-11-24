using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace Data
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options) { }
        public DbSet<Administrador> Administrador { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Guest> Guest { get; set; }
        public DbSet<GuestCart> GuestCart { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductSize> ProductSize { get; set; }
        public DbSet<Size> Size { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Administrador>(entity =>
            {
                entity.ToTable("Administrador");
            });
            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
            });
            builder.Entity<Guest>(entity =>
            {
                entity.ToTable("Guest");
            });
            builder.Entity<GuestCart>(entity =>
            {
                entity.ToTable("GuestCart");
            });

            builder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
            });

            builder.Entity<ProductSize>(entity =>
            {
                entity.ToTable("ProductSize");
            });

            builder.Entity<Size>(entity =>
            {
                entity.ToTable("Size");
            });
        }

    }

    public class ServiceContextFactory : IDesignTimeDbContextFactory<ServiceContext>
    {
        public ServiceContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", false, true);
            var config = builder.Build();
            var connectionString = config.GetConnectionString("ServiceContext");
            var optionsBuilder = new DbContextOptionsBuilder<ServiceContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("ServiceContext"));

            return new ServiceContext(optionsBuilder.Options);
        }
    }


}
