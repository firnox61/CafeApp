//using DataAccess.Migrations;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

       /* public DataContext()
        {
        }*/

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<ProductIngredient> ProductIngredients => Set<ProductIngredient>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Table> Tables => Set<Table>();
        public DbSet<Payment> Payments => Set<Payment>(); 
         public DbSet<ProductionHistory> ProductionHistories => Set<ProductionHistory>();
        public DbSet<User> Users => Set<User>();
        public DbSet<OperationClaim> OperationClaims => Set<OperationClaim>();
        public DbSet<UserOperationClaim> UserOperationClaims => Set<UserOperationClaim>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key
            modelBuilder.Entity<ProductIngredient>()
         .HasKey(pi => new { pi.ProductId, pi.IngredientId });

            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);

            // Decimal precision ayarları
            modelBuilder.Entity<OrderItem>()
                .Property(o => o.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductionHistory>()
                 .HasOne(ph => ph.Product)
                 .WithMany(p => p.ProductionHistories)
                 .HasForeignKey(ph => ph.ProductId)
                 .OnDelete(DeleteBehavior.Restrict); // ✅ ürün silinemez, çünkü geçmiş korunmalı

            modelBuilder.Entity<UserOperationClaim>()
        .HasOne(uoc => uoc.User)
        .WithMany(u => u.UserOperationClaims) // 👈 navigation property kullanılıyor
        .HasForeignKey(uoc => uoc.UserId);

            modelBuilder.Entity<UserOperationClaim>()
                .HasOne(uoc => uoc.OperationClaim)
                .WithMany(oc => oc.UserOperationClaims) // 👈 navigation property kullanılıyor
                .HasForeignKey(uoc => uoc.OperationClaimId);

            // (Opsiyonel) Kullanıcı e-mail unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // (Opsiyonel) Varsayılan roller
            modelBuilder.Entity<OperationClaim>().HasData(
                new OperationClaim { Id = 1, Name = "Admin" },
                new OperationClaim { Id = 2, Name = "Garson" }
            );
            // Diğer konfigürasyonlar

            /*  var now = DateTime.UtcNow;

              modelBuilder.Entity<Ingredient>().HasData(
                  new Ingredient { Id = 1, Name = "Un", Unit = "Kg", Stock = 100, MinStockThreshold = 10 },
                  new Ingredient { Id = 2, Name = "Şeker", Unit = "Kg", Stock = 80, MinStockThreshold = 10 },
                  new Ingredient { Id = 3, Name = "Tuz", Unit = "Kg", Stock = 60, MinStockThreshold = 5 },
                  new Ingredient { Id = 4, Name = "Yağ", Unit = "Litre", Stock = 50, MinStockThreshold = 5 }
              );

              modelBuilder.Entity<Table>().HasData(
                  new Table { Id = 1, Name = "Masa 1" },
                  new Table { Id = 2, Name = "Masa 2" },
                  new Table { Id = 3, Name = "Masa 3" }
              );*/

            /*modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Kurabiye", Description = "Tatlı unlu mamül", Price = 25, Stock = 30, MinStockThreshold = 5, CreatedAt = now },
                new Product { Id = 2, Name = "Poğaça", Description = "Tuzlu hamur işi", Price = 20, Stock = 40, MinStockThreshold = 5, CreatedAt = now }
            );*/

            /*modelBuilder.Entity<ProductIngredient>().HasData(
                new ProductIngredient { ProductId = 1, IngredientId = 1, QuantityRequired = 2 },
                new ProductIngredient { ProductId = 1, IngredientId = 2, QuantityRequired = 1 },
                new ProductIngredient { ProductId = 2, IngredientId = 1, QuantityRequired = 2 },
                new ProductIngredient { ProductId = 2, IngredientId = 3, QuantityRequired = 1 },
                new ProductIngredient { ProductId = 2, IngredientId = 4, QuantityRequired = 1 }
            );*/
        }

    }
}
//Add-Migration InitialCreate

//update-database "InitialCreate"