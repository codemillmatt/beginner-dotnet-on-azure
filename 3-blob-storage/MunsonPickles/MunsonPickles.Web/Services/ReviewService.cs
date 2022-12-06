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

	public async Task<Review> AddReview(Review review)
	{
		try
		{
			pickleContext.Update(review);

			await pickleContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex);
		}

		return review;
	}

	public async Task<IEnumerable<Review>> GetReviewsForProduct(int productId)
	{
		return await pickleContext.Reviews.AsNoTracking().Where(r => r.Product.Id == productId).ToListAsync();
	}
}
