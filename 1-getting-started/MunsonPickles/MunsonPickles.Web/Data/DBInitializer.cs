using MunsonPickles.Web.Models;

namespace MunsonPickles.Web.Data;

public static class DBInitializer
{
    public static void InitializeProducts(ProductContext context)
    {
        if (context.Products.Any())
            return;

        var pickleType = new ProductType { Name = "Pickle" };
        var preserveType = new ProductType { Name = "Preserves" };

        var dillPickles = new Product
        {
            Description = "Deliciously dill",
            Name = "Dill Pickles",
            ProductType = pickleType
        };

        var pickledBeet = new Product
        {
            Description = "unBeetable",
            Name = "Red Pickled Beets",
            ProductType = pickleType
        };

        var strawberryPreserves = new Product
        {
            Description = "Sweet and a treat to make your toast the most",
            Name = "Strawberry Preserves",
            ProductType = preserveType
        };

        context.Add(pickleType);
        context.Add(preserveType);
        context.Add(strawberryPreserves);
        context.Add(dillPickles);
        context.Add(pickledBeet);

        context.SaveChanges();
    }
}
