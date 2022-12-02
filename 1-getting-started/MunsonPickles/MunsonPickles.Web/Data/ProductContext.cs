using Microsoft.EntityFrameworkCore;
using MunsonPickles.Web.Models;

namespace MunsonPickles.Web.Data;

public class ProductContext : DbContext
{
	public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

	public DbSet<Product> Products => Set<Product>();
	public DbSet<ProductType> ProductTypes => Set<ProductType>();
}

public static class Extensions
{
	public static void CreateDbIfNotExists(this IHost host)
	{
		using var scope = host.Services.CreateScope();

		var services = scope.ServiceProvider;
		var context = services.GetRequiredService<ProductContext>();
		context.Database.EnsureCreated();

		DBInitializer.InitializeProducts(context);
	}
}
