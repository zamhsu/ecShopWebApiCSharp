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
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Services
builder.Services.AddScoped<IAdminMemberService, AdminMemberService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductCategoryTypeService, ProductCategoryTypeService>();
builder.Services.AddScoped<IProductUnitTypeService, ProductUnitTypeService>();

builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

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
