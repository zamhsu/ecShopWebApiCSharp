using Common.Implements;
using Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Contexts;
using Service.Mappings;
using System.Text;
using Microsoft.Data.Sqlite;
using WebApi.Infrastructures.Core;
using WebApi.Infrastructures.Helpers;
using WebApi.Infrastructures.ServiceCollectionExtensions;
using WebApi.Infrastructures.Mappings;

var allowSpecificOrigins = "_allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// CORS
builder.Services.AddCors(opt =>
{
    // 從appsettings.json中取得允許的來源
    string[] allowOrigins = builder.Configuration.GetValue<string>("Cors:AllowOrigins").Split(',', StringSplitOptions.RemoveEmptyEntries);
    opt.AddPolicy(name: allowSpecificOrigins,
                  b =>
                  {
                      b.WithOrigins(allowOrigins)
                       .AllowAnyHeader()
                       .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
                  });
});

// Add services to the container.

builder.Services.AddControllers();

var cnn = new SqliteConnection(builder.Configuration.GetConnectionString("SQLiteConnection"));
cnn.Open();
builder.Services.AddDbContext<EcShopContext>(o => o.UseSqlite(cnn));

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

// 自定義的DI
builder.Services.AddServiceDependency();

builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
builder.Services.AddSingleton<JwtHelper>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

SystemInitialization.AddDefaultData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 在app.UseRouting()後，app.UseAuthentication()前
app.UseCors(allowSpecificOrigins);

// 先驗證
app.UseAuthentication();
// 再授權
app.UseAuthorization();

app.MapControllers();

app.Run();