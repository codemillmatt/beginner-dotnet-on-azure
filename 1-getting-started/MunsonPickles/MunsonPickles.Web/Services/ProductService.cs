using Microsoft.EntityFrameworkCore;
using MunsonPickles.Web.Data;
using MunsonPickles.Web.Models;

namespace MunsonPickles.Web.Services;

public class ProductService
{
    private readonly ProductContext productContext;

    public ProductService(ProductContext context)
    {
        productContext = context;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await productContext
            .Products
            .Include(p => p.ProductType)
            .AsNoTracking()
            .ToListAsync();
    }
}
