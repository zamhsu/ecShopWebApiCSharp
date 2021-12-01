using Microsoft.EntityFrameworkCore;
using WebApi.Base;
using WebApi.Base.IRepositories;
using WebApi.Base.Repositories;
using WebApi.Base.Mappings;
using WebApi.Base.IServices.Members;
using WebApi.Base.IServices.Products;
using WebApi.Base.Services.Members;
using WebApi.Base.Services.Products;
using WebApi.Models;
using WebApi.Base.IServices.Security;
using WebApi.Base.Services.Security;
using WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<EcShopContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));
builder.Services.AddScoped<DbContext, EcShopContext>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ServicesProfile>();
    cfg.AddProfile<ControllersProfile>();
});

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
        opt.IncludeErrorDetails = true;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

            // 驗證Issuer
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),

            // 不太驗證Audience
            ValidateAudience = false,

            // 驗證Token有效期
            ValidateLifetime = true,

            // 如果Token中包含key才需要驗證，一般都只有簽章而已
            ValidateIssuerSigningKey = false,
            // 從IConfiguration取得IssuerSigningKey
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
        };
    });

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IAdminMemberService, AdminMemberService>();
builder.Services.AddScoped<IAdminMemberAccountService, AdminMemberAccountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductCategoryTypeService, ProductCategoryTypeService>();
builder.Services.AddScoped<IProductUnitTypeService, ProductUnitTypeService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();

builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
builder.Services.AddSingleton<JwtHelper>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 先驗證
app.UseAuthentication();
// 再授權
app.UseAuthorization();

app.MapControllers();

app.Run();
