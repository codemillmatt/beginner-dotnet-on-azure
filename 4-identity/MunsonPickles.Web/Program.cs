using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Azure;
using MunsonPickles.Web.Services;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var storageConnection = builder.Configuration["ConnectionStrings:Storage:DotAzure"];

builder.Services.AddAzureClients(azureBuilder =>
{
    azureBuilder.AddBlobServiceClient(storageConnection);
});

builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<ReviewService>();

builder.Services.AddHttpClient<ProductService>(client =>
{
    string productUrl = builder.Configuration["Api:ProductEndpoint"] ?? "http://localhost:3500";

    client.BaseAddress = new Uri(productUrl);
});

builder.Services.AddHttpClient<ReviewService>(client =>
{
    string reviewUrl = builder.Configuration["Api:ReviewEndpoint"] ?? "http://localhost:3500";

    client.BaseAddress = new Uri(reviewUrl);
});

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
