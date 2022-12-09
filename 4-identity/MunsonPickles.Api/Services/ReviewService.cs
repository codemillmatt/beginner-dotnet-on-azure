using Microsoft.EntityFrameworkCore;
using MunsonPickles.Api.Data;
using MunsonPickles.Shared.Models;

namespace MunsonPickles.Api.Services;

public class ReviewService
{
    private readonly PickleDbContext pickleContext;
    

    public ReviewService(PickleDbContext context)
    {
        pickleContext = context;        
    }

    public async Task AddReview(string reviewText, List<string> photoUrls, int productId)
    {
        string userId = "matt"; // this will get changed out when we add auth

        try
        {
            // create all the photo url object
            List<ReviewPhoto> photos = new List<ReviewPhoto>();

            foreach (var photoUrl in photoUrls)
            {
                photos.Add(new ReviewPhoto { PhotoUrl = photoUrl });
            }

            // create the new review
            Review review = new()
            {
                Date = DateTime.Now,
                Photos = photos,
                Text = reviewText,
                UserId = userId
            };

            Product product = await pickleContext
                .Products
                .Include(p => p.Reviews)
                .FirstAsync(p => p.Id == productId);

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

    public async Task<Review>? GetReviewById(int reviewId)
    {
        return await pickleContext
            .Reviews
            .Include(r => r.Product)
            .Include(r => r.Photos)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == reviewId);
    }
}
