using Repository.Contexts;
using Repository.Entities.Members;
using Repository.Entities.Orders;
using Repository.Entities.Products;

namespace WebApi.Infrastructures.Core;

public static class SystemInitialization
{
    /// <summary>
    /// 加入預設的資料
    /// </summary>
    /// <param name="app">WebApplication</param>
    public static void AddDefaultData(WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<EcShopContext>();
        
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var productStatusOk = new ProductStatus()
        {
            Id = 1,
            Name = "Ok"
        };

        var productStatusDelete = new ProductStatus()
        {
            Id = 2,
            Name = "Delete"
        };

        db.ProductStatus.Add(productStatusOk);
        db.ProductStatus.Add(productStatusDelete);

        var productUnitType = new ProductUnitType()
        {
            Id = 1,
            Name = "台",
            Deleted = false
        };

        db.ProductUnitType.Add(productUnitType);

        var productCategoryType = new ProductCategoryType()
        {
            Id = 1,
            Name = "3C",
            Deleted = false
        };

        db.ProductCategoryType.Add(productCategoryType);
        
        db.SaveChanges();

        var product1 = new Product()
        {
            Id = 1,
            Guid = "7cde4221-605f-451c-b7d0-ed549c260efb",
            UnitId = 1,
            CategoryId = 1,
            Title = "iPad Pro",
            Description = "平板電腦",
            ImageUrl = "https://memeprod.sgp1.digitaloceanspaces.com/user-wtf/1701408346072.jpg",
            Memo = "庫存不多",
            Quantity = 10,
            OriginPrice = 68000,
            Price = 50000,
            CreateDate = DateTimeOffset.UtcNow.ToUniversalTime(),
            StartDisplay = DateTimeOffset.UtcNow.ToUniversalTime(),
            EndDisplay = DateTimeOffset.UtcNow.AddYears(10).ToUniversalTime(),
            
            ProductStatus = productStatusOk,
            ProductUnitType = productUnitType,
            ProductCategoryType = productCategoryType
        };
        
        var product2 = new Product()
        {
            Id = 2,
            Guid = "75ad5ed3-1b99-4954-9533-7bf2660cb3ca\n",
            UnitId = 1,
            CategoryId = 1,
            Title = "iPad Air",
            Description = "平板電腦",
            ImageUrl = "https://memeprod.sgp1.digitaloceanspaces.com/user-wtf/1701863608564.jpg",
            Memo = "庫存很多",
            Quantity = 100,
            OriginPrice = 38000,
            Price = 20000,
            CreateDate = DateTimeOffset.UtcNow.ToUniversalTime(),
            StartDisplay = DateTimeOffset.UtcNow.ToUniversalTime(),
            EndDisplay = DateTimeOffset.UtcNow.AddYears(10).ToUniversalTime(),
            
            ProductStatus = productStatusOk,
            ProductUnitType = productUnitType,
            ProductCategoryType = productCategoryType
        };
        
        db.Product.Add(product1);
        db.Product.Add(product2);
        
        db.SaveChanges();

        var adminMemberStatusOk = new AdminMemberStatus()
        {
            Id = 1,
            Name = "Ok"
        };
        
        var adminMemberStatusStop = new AdminMemberStatus()
        {
            Id = 2,
            Name = "Stop"
        };
        
        var adminMemberStatusDelete = new AdminMemberStatus()
        {
            Id = 3,
            Name = "Delete"
        };
        
        var adminMemberStatusLock = new AdminMemberStatus()
        {
            Id = 4,
            Name = "Lock"
        };

        db.AdminMemberStatus.Add(adminMemberStatusOk);
        db.AdminMemberStatus.Add(adminMemberStatusStop);
        db.AdminMemberStatus.Add(adminMemberStatusDelete);
        db.AdminMemberStatus.Add(adminMemberStatusLock);

        db.SaveChanges();

        var adminMember = new AdminMember()
        {
            Id = 1,
            Guid = "ca7bcefd-2ecf-4b57-911a-08f5b770d834",
            UserName = "admin",
            Email = "admin@example.com",
            Account = "admin",
            Pwd = "295395022C7BD9622A1547507B5BFB29A31C6F2148EFAB2846B4D387BC7C1459", // ecShopAdmin
            HashSalt = "bc193c4c-0d75-4b5f-9b3b-c1dab8cc4880",
            StatusId = 1,
            ErrorTimes = 0,
            LastLoginDate = DateTimeOffset.UtcNow.AddDays(10).ToUniversalTime(),
            IsMaster = true,
            ExpirationDate = DateTimeOffset.UtcNow.AddYears(10).ToUniversalTime(),
            CreateDate = DateTimeOffset.UtcNow.ToUniversalTime(),
            
            AdminMemberStatus = adminMemberStatusOk
        };

        db.AdminMember.Add(adminMember);

        db.SaveChanges();

        var orderStatusPlaceOrder = new OrderStatus()
        {
            Id = 1,
            Name = "建立訂單"
        };

        var orderStatusPaymentSuccessful = new OrderStatus()
        {
            Id = 2,
            Name = "完成付款"
        };

        var orderStatusPaymentFailed = new OrderStatus()
        {
            Id = 3,
            Name = "付款失敗"
        };

        db.OrderStatuse.Add(orderStatusPlaceOrder);
        db.OrderStatuse.Add(orderStatusPaymentSuccessful);
        db.OrderStatuse.Add(orderStatusPaymentFailed);

        db.SaveChanges();

        var paymentMethodCreditCard = new PaymentMethod()
        {
            Id = 1,
            Name = "CreditCard"
        };
        
        var paymentMethodAtm = new PaymentMethod()
        {
            Id = 2,
            Name = "ATM"
        };

        db.PaymentMethod.Add(paymentMethodCreditCard);
        db.PaymentMethod.Add(paymentMethodAtm);

        db.SaveChanges();
    }
}