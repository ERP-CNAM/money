using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using MoneyApp.Data;
using DotNetEnv;

using MoneyApp.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Services framework
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Services métier
builder.Services.AddScoped<JsonDataService>();
builder.Services.AddSingleton<AccountingGenerator>();

// Service telechargement
builder.Services.AddScoped<MoneyApp.Services.CSVBuilder>();

//service bdd
builder.Services.AddScoped<ExportService>();
builder.Services.AddScoped<ImportService>();

builder.Services.AddScoped<DataSyncService>();

builder.Services.AddHttpClient<ExternalConnectService>();
builder.Services.AddScoped<ExternalConnectService>();

//db connection
var connectionString = "Server=" + Environment.GetEnvironmentVariable("DATABASE_URL") + ";Port=" + Environment.GetEnvironmentVariable("DATABASE_PORT") + ";Database=" + Environment.GetEnvironmentVariable("DATABASE_DATABASE") + "; User=" + Environment.GetEnvironmentVariable("DATABASE_USER")+ ";Password=" + Environment.GetEnvironmentVariable("DATABASE_PASSWORD") + ";"; //TODO : recuperer du .env
Console.WriteLine(connectionString);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage.ProtectedSessionStorage>();

// Stockage JWT côté navigateur (Blazor Server)
builder.Services.AddScoped<ProtectedSessionStorage>();

// Auth + gateway
builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<ConnectGateway>();
builder.Services.AddScoped<AuthService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine($"Invoices en DB : {db.Invoices.Count()}");
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
