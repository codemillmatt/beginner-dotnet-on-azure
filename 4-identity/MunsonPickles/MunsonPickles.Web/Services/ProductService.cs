using MunsonPickles.Shared.Models;

namespace MunsonPickles.Web.Services;

public class ProductService
{
    private readonly HttpClient productHttp;

    public ProductService(HttpClient httpClient)
    {
        productHttp = httpClient;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await productHttp.GetFromJsonAsync<IEnumerable<Product>>("/products");
    }

    public async Task<Product>? GetProductById(int productId)
    {
        return await productHttp.GetFromJsonAsync<Product>($"/products/{productId}");
    }
}
