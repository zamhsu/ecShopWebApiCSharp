using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebApi.Models.Products;

namespace WebApi.Models
{
    public class EcShopContext : DbContext
    {
        public EcShopContext(DbContextOptions<EcShopContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; } = null!;
        public DbSet<ProductCategoryType> ProductCategoryType { get; set; } = null!;
        public DbSet<ProductUnitType> ProductUnitType { get; set; } = null!;
        public DbSet<ProductStatus> ProductStatus { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(e =>
            {
                e.HasIndex(b => b.Guid)
                    .IsUnique();

                e.HasOne(b => b.ProductCategoryType)
                    .WithMany(p => p.Product)
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_Product_CategoryId_To_ProductCategoryType_Id");

                e.HasOne(b => b.ProductUnitType)
                    .WithMany(p => p.Product)
                    .HasForeignKey(b => b.UnitId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_Product_UnitId_To_ProductUnitType_Id");

                e.HasOne(b => b.ProductStatus)
                    .WithMany(p => p.Product)
                    .HasForeignKey(b => b.StatusId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_Product_StatusId_To_ProductStatus_Id");

                e.ToTable("Product");
            });

            modelBuilder.Entity<ProductCategoryType>(e =>
            {
                e.Property(p => p.Deleted)
                    .HasDefaultValue(false)
                    .IsRequired();
            });

            modelBuilder.Entity<ProductUnitType>(e =>
            {
                e.Property(p => p.Deleted)
                    .HasDefaultValue(false)
                    .IsRequired();
            });
        }
    }
}