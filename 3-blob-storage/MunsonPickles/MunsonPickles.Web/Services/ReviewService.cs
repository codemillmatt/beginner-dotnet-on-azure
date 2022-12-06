using MunsonPickles.Web.Data;
using MunsonPickles.Web.Models;

using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MunsonPickles.Web.Services;

public class ReviewService
{
	private readonly PickleDbContext pickleContext;
	private readonly BlobServiceClient blobServiceClient;

	public ReviewService(PickleDbContext context, BlobServiceClient blob)
	{
		pickleContext = context;
		blobServiceClient = blob;
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
				photos.Add(new ReviewPhoto {  PhotoUrl = photoUrl });
			}

			// create the new review
			Review review = new()
			{
				Date = DateTime.Now,
				Photos = photos,
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
