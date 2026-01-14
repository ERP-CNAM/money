using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MoneyApp.Data;
using MoneyApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Services framework
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Services métier
builder.Services.AddSingleton<JsonDataService>();
builder.Services.AddSingleton<AccountingGenerator>();

// Service telechargement
builder.Services.AddScoped<MoneyApp.Services.CSVBuilder>();

//service bdd
builder.Services.AddScoped<ExportService>();

//db connection
var connectionString =
    "Server=localhost;Port=3306;Database=moneyapp;User=moneyapp;Password=moneyapp;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine($"Invoices en DB : {db.Invoices.Count()}");
}

//db data 
using (var scope = app.Services.CreateScope())
{
    var exporter = scope.ServiceProvider.GetRequiredService<ExportService>();

    await exporter.ExportAllAsync("Data/invoices.json", "Data/payments.json");
    Console.WriteLine("Export JSON terminé !");
}

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
