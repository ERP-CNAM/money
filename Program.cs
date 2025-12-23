using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MoneyApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Services framework
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Services métier
builder.Services.AddSingleton<JsonDataService>();
builder.Services.AddSingleton<AccountingGenerator>();

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
