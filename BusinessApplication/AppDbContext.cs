using BusinessApplication.Model;
using Microsoft.EntityFrameworkCore;

namespace BusinessApplicationProject
{
    public class AppDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleGroup> ArticleGroups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=BusinessApplicationDb;User Id=sa;Password=Password123;Encrypt=no");
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Configure Navigation Properties

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Group)
                .WithMany()
                .HasForeignKey(g => g.Id)
                .IsRequired(false);

            modelBuilder.Entity<ArticleGroup>()
                .HasOne(a => a.Parent)
                .WithMany()
                .HasForeignKey(p => p.Id)
                .IsRequired(false);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.CustomerAddress)
                .WithMany()
                .HasForeignKey(a => a.Id);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.BillingAddress)
                .WithMany()
                .HasForeignKey(b => b.Id);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.OrderInformations)
                .WithMany()
                .HasForeignKey(o => o.Id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.CustomerDetails)
                .WithMany()
                .HasForeignKey(c => c.Id);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Positions)
                .WithOne()
                .HasForeignKey(p => p.Id);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.ArticleDetails)
                .WithMany()
                .HasForeignKey(a => a.Id);

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
