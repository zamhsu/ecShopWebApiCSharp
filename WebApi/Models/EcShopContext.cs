using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebApi.Models.Members;
using WebApi.Models.Orders;
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

        // Orders
        public DbSet<Order> Order { get; set; } = null!;
        public DbSet<OrderStatus> OrderStatuse { get; set; } = null!;
        public DbSet<Coupon> Coupon { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetail { get; set; } = null!;
        public DbSet<PaymentMethod> PaymentMethod { get; set; } = null!;

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

            // Orders
            modelBuilder.Entity<Order>(e =>
            {
                e.HasIndex(b => b.Guid)
                    .IsUnique();

                e.HasOne(b => b.PaymentMethod)
                    .WithMany(p => p.Order)
                    .HasForeignKey(b => b.PaymentMethodId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Order_PaymentMethodId_To_PaymentMethod_Id");

                e.HasOne(b => b.OrderStatus)
                    .WithMany(p => p.Order)
                    .HasForeignKey(b => b.StatusId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_Order_StatusId_To_OrderStatus_Id");

                e.ToTable("Order");
            });

            modelBuilder.Entity<OrderDetail>(e =>
            {
                e.HasKey(b => new { b.OrderId, b.ItemNo });

                e.HasOne(b => b.Order)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(b => b.OrderId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_OrderDetail_OrderId_To_Order_Id");

                e.HasOne(b => b.Product)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(b => b.ProductId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_OrderDetail_ProductId_To_Product_Id");

                e.HasOne(b => b.Coupon)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(b => b.CouponId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_OrderDetail_CouponId_To_Coupon_Id");

                e.ToTable("OrderDetail");
            });

            modelBuilder.Entity<Coupon>(e =>
            {
                e.HasOne(b => b.CouponStatus)
                    .WithMany(p => p.Coupon)
                    .HasForeignKey(b => b.StatusId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_Coupon_StatusId_To_CouponStatus_Id");

                e.ToTable("Coupon");
            });

            modelBuilder.Entity<OrderStatus>(e =>
            {
                e.ToTable("OrderStatus");
            });
        }
    }
}