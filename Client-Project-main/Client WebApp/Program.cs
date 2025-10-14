using Client.API.Controllers;
using Client.Application.Interfaces;
using Client.Persistence.Context;
using Client.Persistence.Repositories;
using Client_WebApp.Middleware;
using Client_WebApp.Services;
using Client_WebApp.Services.Master;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Add session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// Register repository and services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<ISubContractorRepository, SubContractorRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IBankMasterRepository, BankMasterRepository>();

builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<BankService>();
builder.Services.AddScoped<SubContractorService>();

// IDbConnection
builder.Services.AddScoped<IDbConnection>(sp =>
    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// IEmailService (example)
builder.Services.AddScoped<IEmailService, EmailService>();

// IJwtService
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

//app.UseMiddleware<AuthMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
