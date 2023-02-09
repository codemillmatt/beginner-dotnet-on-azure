﻿using Microsoft.EntityFrameworkCore;
using MunsonPickles.Api.Data;
using MunsonPickles.Shared.Models;

namespace MunsonPickles.Api.Services;

public class ProductService
{
    private readonly PickleDbContext productContext;

    public ProductService(PickleDbContext context)
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

    public async Task<Product>? GetProductById(int productId)
    {
        return await productContext
            .Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId);
    }
}
