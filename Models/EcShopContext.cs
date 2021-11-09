using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebApi.Models.Members;
using WebApi.Models.Products;

namespace WebApi.Models
{
    public class EcShopContext : DbContext
    {
        public EcShopContext(DbContextOptions<EcShopContext> options)
            : base(options)
        {
        }

        // Products
        public DbSet<Product> Product { get; set; } = null!;
        public DbSet<ProductCategoryType> ProductCategoryType { get; set; } = null!;
        public DbSet<ProductUnitType> ProductUnitType { get; set; } = null!;
        public DbSet<ProductStatus> ProductStatus { get; set; } = null!;

        // Members
        public DbSet<AdminMember> AdminMember { get; set; } = null!;
        public DbSet<AdminMemberStatus> AdminMemberStatus { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Products
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

            // Members
            modelBuilder.Entity<AdminMember>(e =>
            {
                e.HasIndex(b => b.Guid)
                    .IsUnique();

                e.HasOne(b => b.AdminMemberStatus)
                    .WithMany(p => p.AdminMember)
                    .HasForeignKey(b => b.StatusId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_AdminMember_StatusId_To_AdminMemberStatus_Id");

                e.Property(p => p.IsMaster)
                    .HasDefaultValue(false)
                    .IsRequired();

                e.ToTable("AdminMember");
            });
        }
    }
}