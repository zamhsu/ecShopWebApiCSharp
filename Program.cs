using AutoMapper;
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

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<EcShopContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<ServicesProfile>();
    cfg.AddProfile<ControllersProfile>();
});

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IAdminMemberRepository, AdminMemberRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
