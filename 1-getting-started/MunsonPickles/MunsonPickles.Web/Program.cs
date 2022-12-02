using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MunsonPickles.Web.Data;
using MunsonPickles.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var sqlConnection = builder.Configuration["ConnectionStrings:SqlDb:DotAzure"];

builder.Services.AddSqlServer<ProductContext>(sqlConnection, options => options.EnableRetryOnFailure());
builder.Services.AddTransient<ProductService>();

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

app.CreateDbIfNotExists();

app.Run();
