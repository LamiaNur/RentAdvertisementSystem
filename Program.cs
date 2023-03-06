using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RentAdvertisementSystem.Data;
using RentAdvertisementSystem.Database;
using RentAdvertisementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<LocalStorage>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<MongoDbClient>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<RentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
