using MunsonPickles.Web.Data;
using MunsonPickles.Web.Models;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace MunsonPickles.Web.Services;

public class ReviewService
{
	private readonly PickleDbContext pickleContext;

	public ReviewService(PickleDbContext context)
	{
		pickleContext = context;
	}

	public async Task AddReview(string reviewText, int productId)
	{
        string userId = "matt"; // this will get changed out when we add auth

        try
		{
            // create the new review
            Review review = new()
            {
                Date = DateTime.Now,                
                Text = reviewText,
                UserId = userId
            };

            Product product = await pickleContext.Products.FindAsync(productId);

            if (product is null)
                return;

            if (product.Reviews is null)
                product.Reviews = new List<Review>();

            product.Reviews.Add(review);

            await pickleContext.SaveChangesAsync();
        }
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex);
		}		
	}

	public async Task<IEnumerable<Review>> GetReviewsForProduct(int productId)
	{
		return await pickleContext.Reviews.AsNoTracking().Where(r => r.Product.Id == productId).ToListAsync();
	}
}
