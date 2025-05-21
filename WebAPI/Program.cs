using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IProductDal, EfProductDal>();

builder.Services.AddScoped<IPaymentService, PaymentManager>();
builder.Services.AddScoped<IPaymentDal, EfPaymentDal>();

builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddScoped<IOrderDal, EfOrderDal>();

builder.Services.AddScoped<IIngredientService, IngredientManager>();
builder.Services.AddScoped<IIngredientDal, EfIngredientDal>();

builder.Services.AddScoped<ITableService, TableManager>();
builder.Services.AddScoped<ITableDal, EfTableDal>();

builder.Services.AddScoped<IOrderItemService, OrderItemManager>();
builder.Services.AddScoped<IOrderItemDal, EfOrderItemDal>();

builder.Services.AddScoped<IProductIngredientService, ProductIngredientManager>();
builder.Services.AddScoped<IProductIngredientDal, EfProductIngredientDal>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
