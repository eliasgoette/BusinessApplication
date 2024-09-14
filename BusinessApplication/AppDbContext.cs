using BusinessApplication.Model;
using Microsoft.EntityFrameworkCore;

namespace BusinessApplication
{
    public delegate DbContext DbContextFactoryMethod();

    public class AppDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Using username and password
                optionsBuilder.UseSqlServer("Server=localhost,1433;Database=BusinessApplicationDb;User Id=sa;Password=Password123;Encrypt=no");
                // Using Microsoft Account:
                // optionsBuilder.UseSqlServer("Server=BT,1433; Database=BusinessApplicationDb; Trusted_Connection=True; Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .Navigation(e => e.CustomerAddress)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Address>().ToTable(nameof(Addresses), b => b.IsTemporal());
            modelBuilder.Entity<Article>().ToTable(nameof(Articles), b => b.IsTemporal());
            modelBuilder.Entity<Customer>().ToTable(nameof(Customers), b => b.IsTemporal());

            modelBuilder.Entity<Article>().HasData(
                new Article
                {
                    Id = 1,
                    ArticleNumber = "A00001",
                    Name = "Apple MacBook Pro",
                    Price = 2800
                },
                new Article
                {
                    Id = 2,
                    ArticleNumber = "A00002",
                    Name = "Google Pixel Fold",
                    Price = 1950
                },
                new Article
                {
                    Id = 3,
                    ArticleNumber = "A00003",
                    Name = "Samsung Galaxy Book Pro",
                    Price = 1800
                },
                new Article
                {
                    Id = 4,
                    ArticleNumber = "A00004",
                    Name = "Apple iPhone 15 Pro",
                    Price = 1200
                },
                new Article
                {
                    Id = 5,
                    ArticleNumber = "A00005",
                    Name = "Apple Polishing Cloth",
                    Price = 39.99
                }
            );
        }
    }
}