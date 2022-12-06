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

        var dillReview = new Review
        {
            Date = DateTime.Now,
            Text = "These pickles pack a punch",
            UserId = "matt"
        };

        var dillPickles = new Product
        {
            Description = "Deliciously dill",
            Name = "Dill Pickles",
            ProductType = pickleType,
            Reviews = new List<Review> { dillReview }
        };

        var beetReview = new Review
        {
            Date = DateTime.Now,
            Text = "Bonafide best beets",
            UserId = "matt"
        };

        var pickledBeet = new Product
        {
            Description = "unBeetable",
            Name = "Red Pickled Beets",
            ProductType = pickleType,
            Reviews = new List<Review> { beetReview }
        };

        var preserveReview = new Review
        {
            Date = DateTime.Now,
            Text = "Succulent strawberries making biscuits better",
            UserId = "matt"
        };

        var strawberryPreserves = new Product
        {
            Description = "Sweet and a treat to make your toast the most",
            Name = "Strawberry Preserves",
            ProductType = preserveType,
            Reviews = new List<Review> { preserveReview }
        };

        pickleContext.Products.Add(dillPickles);
        pickleContext.Products.Add(pickledBeet);
        pickleContext.Products.Add(strawberryPreserves);

        pickleContext.SaveChanges();
    }
}
