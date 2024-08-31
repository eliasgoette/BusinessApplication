using BusinessApplication.Model;
using Microsoft.EntityFrameworkCore;

namespace BusinessApplication
{
    public delegate DbContext DbContextFactoryMethod();

    public class AppDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleGroup> ArticleGroups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost,1433;Database=BusinessApplicationDb;User Id=sa;Password=Password123;Encrypt=no");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Configure Navigation Properties

            modelBuilder.Entity<Article>()
                .Navigation(e => e.Group)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<ArticleGroup>()
                .Navigation(e => e.Parent)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<ArticleGroup>()
                .Navigation(e => e.Articles)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Customer>()
                .Navigation(e => e.CustomerAddress)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Invoice>()
                .Navigation(e => e.BillingAddress)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Invoice>()
                .Navigation(e => e.OrderInformations)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Order>()
                .Navigation(e => e.CustomerDetails)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Order>()
                .Navigation(e => e.Positions)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Position>()
                .Navigation(e => e.ArticleDetails)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            #endregion

            modelBuilder.Entity<Address>().ToTable(nameof(Addresses), b => b.IsTemporal());
            modelBuilder.Entity<Article>().ToTable(nameof(Articles), b => b.IsTemporal());
            modelBuilder.Entity<ArticleGroup>().ToTable(nameof(ArticleGroups), b => b.IsTemporal());
            modelBuilder.Entity<Customer>().ToTable(nameof(Customers), b => b.IsTemporal());
            modelBuilder.Entity<Invoice>().ToTable(nameof(Invoices), b => b.IsTemporal());
            modelBuilder.Entity<Order>().ToTable(nameof(Orders), b => b.IsTemporal());
            modelBuilder.Entity<Position>().ToTable(nameof(Positions), b => b.IsTemporal());
        }
    }
}