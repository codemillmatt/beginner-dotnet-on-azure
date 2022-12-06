using Microsoft.AspNetCore.Mvc.Rendering;
using MunsonPickles.Web.Models;

namespace MunsonPickles.Web.Data;

public static class DBInitializer
{
    public static void InitializeDatabase(PickleDbContext pickleContext)
    {
        if (pickleContext.Products.Any())
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
            ProductType = pickleType,            
        };

        var strawberryPreserves = new Product
        {
            Description = "Sweet and a treat to make your toast the most",
            Name = "Strawberry Preserves",
            ProductType = preserveType
        };

        pickleContext.Add(pickleType);
        pickleContext.Add(preserveType);

        pickleContext.Add(strawberryPreserves);
        pickleContext.Add(dillPickles);
        pickleContext.Add(pickledBeet);

        var dillReview = new Review
        {
            Date = DateTime.Now,
            Product = dillPickles,
            Text = "These pickles pack a punch",
            UserId = "matt"
        };

        var beetReview = new Review
        {
            Date = DateTime.Now,
            Product = pickledBeet,
            Text = "Bonafide best beets",
            UserId = "matt"
        };

        var preserveReview = new Review
        {
            Date = DateTime.Now,
            Product = strawberryPreserves,
            Text = "Succulent strawberries making biscuits better",
            UserId = "matt"
        };

        pickleContext.Add(dillReview);
        pickleContext.Add(preserveReview);
        pickleContext.Add(beetReview);

        pickleContext.SaveChanges(); 
    }
}
