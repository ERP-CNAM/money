using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
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

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage.ProtectedSessionStorage>();

// Stockage JWT côté navigateur (Blazor Server)
builder.Services.AddScoped<ProtectedSessionStorage>();

// Auth + gateway
builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<ConnectGateway>();
builder.Services.AddScoped<AuthService>();



var app = builder.Build();

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
