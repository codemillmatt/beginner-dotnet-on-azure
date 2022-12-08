using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

using MunsonPickles.Api.Data;
using MunsonPickles.Api.Services;
using MunsonPickles.Shared.Models;
using MunsonPickles.Shared.Transfer;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

var initialScopes = builder.Configuration["AzureAdB2C:Scopes"]?.Split(' ');

builder.Services.AddAuthorization();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConnection = builder.Configuration["ConnectionStrings:SqlDb:DotAzure"];
builder.Services.AddSqlServer<PickleDbContext>(sqlConnection, options => options.EnableRetryOnFailure());

builder.Services.AddTransient<ProductService>();
builder.Services.AddTransient<ReviewService>();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
var scopedRequiredByApi = app.Configuration["AzureAdB2C:Scopes"] ?? "";

app.MapGet("/products", async (ProductService productService) =>
{
    return await productService.GetAllProducts();
})
.WithName("GetAllProducts")
.WithOpenApi()
.Produces<IEnumerable<Product>>(StatusCodes.Status200OK);

app.MapGet("/products/{productId}", async (ProductService productService, int productId) =>
{
    return await productService.GetProductById(productId);
})
.WithName("GetProductById")
.WithOpenApi()
.Produces<Product>(StatusCodes.Status200OK);

app.MapGet("/products/{productId}/reviews", async (ReviewService reviewService, int productId) =>
{
    return await reviewService.GetReviewsForProduct(productId);
})
.WithName("GetReviewsForProduct")
.WithOpenApi()
.Produces<IEnumerable<Review>>(StatusCodes.Status200OK);

app.MapGet("/reviews/{reviewId}", async(ReviewService reviewService, int reviewId) => 
{
    return await reviewService.GetReviewById(reviewId);
})
.WithName("GetReviewById")
.WithOpenApi()
.Produces<Review>(StatusCodes.Status200OK);


app.MapPost("/reviews", async (HttpContext httpContext, ReviewService reviewService, NewReview review) =>
{
    await reviewService.AddReview(
        review.ReviewText, review.PhotoUrls, review.ProductId
    );

    return Results.Created("/review", null);
})
.WithName("AddReview")
.WithOpenApi()
.Produces(StatusCodes.Status201Created)
.Produces(StatusCodes.Status401Unauthorized)
.RequireAuthorization();

app.CreateDbIfNotExists();

app.Run();
