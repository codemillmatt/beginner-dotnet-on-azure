
using Microsoft.Extensions.Azure;
using MunsonPickles.Web.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

using Azure.Identity;
using Azure.Core.Extensions;
using Azure.Storage.Queues;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

var initialScopes = builder.Configuration["ReviewApi:Scopes"]?.Split(' ');

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"))
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
    .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
builder.Services.AddRazorPages().AddMicrosoftIdentityUI();
builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;    
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

//var storageConnection = builder.Configuration["ConnectionStrings:Storage:DotAzure"];

var blobStorageUri = builder.Configuration["Storage:Endpoint"];
var queueStorageUri = builder.Configuration["Storage:QueueEndpoint"];

builder.Services.AddAzureClients(azureBuilder =>
{
    var credentials = new DefaultAzureCredential();
    
    azureBuilder.AddBlobServiceClient(new Uri(blobStorageUri));

    var s = azureBuilder.AddQueueServiceClient(new Uri(queueStorageUri)).ConfigureOptions(options =>
    {
        options.MessageEncoding = QueueMessageEncoding.Base64;
    });
    
    
    azureBuilder.UseCredential(credentials);
});

builder.Services.AddSingleton<ProductService>();
builder.Services.AddTransient<ReviewService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
