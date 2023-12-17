using Repository.Implements;
using Repository.Interfaces;
using Service.Implements.Members;
using Service.Implements.Orders;
using Service.Implements.Payments;
using Service.Implements.Products;
using Service.Implements.Security;
using Service.Interfaces.Members;
using Service.Interfaces.Orders;
using Service.Interfaces.Payments;
using Service.Interfaces.Products;
using Service.Interfaces.Security;

namespace WebApi.Infrastructures.ServiceCollectionExtensions;

public static class ServiceDependencyServiceCollectionExtension
{
    /// <summary>
    /// 加入服務需要使用的服務
    /// </summary>
    /// <param name="services">ServiceCollection</param>
    /// <returns></returns>
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        // Unit of Work
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IAdminMemberService, AdminMemberService>();
        services.AddScoped<IAdminMemberAccountService, AdminMemberAccountService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductCategoryTypeService, ProductCategoryTypeService>();
        services.AddScoped<IProductUnitTypeService, ProductUnitTypeService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<IOrderService, OrderSerivce>();
        services.AddScoped<IOrderDetailService, OrderDetailService>();
        services.AddScoped<IOrderAmountService, OrderAmountService>();
        services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IEncryptionService, EncryptionService>();

        return services;
    }
}