using Microsoft.EntityFrameworkCore;
using MunsonPickles.Shared.Models;

namespace MunsonPickles.Api.Data;

public class PickleDbContext : DbContext
{
	public PickleDbContext(DbContextOptions<PickleDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging();
    }

    public DbSet<Product> Products => Set<Product>();
	public DbSet<ProductType> ProductTypes => Set<ProductType>();
	public DbSet<Review> Reviews => Set<Review>();
	public DbSet<ReviewPhoto> ReviewsPhoto => Set<ReviewPhoto>();
}


