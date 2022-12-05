using MunsonPickles.Web.Data;
using MunsonPickles.Web.Models;

namespace MunsonPickles.Web.Services;

public class ReviewService
{
	private readonly PickleDbContext pickleContext;

	public ReviewService(PickleDbContext context)
	{
		pickleContext = context;
	}

	public async Task<Review> AddReview(Review review)
	{
		pickleContext.Add(review);

		await pickleContext.SaveChangesAsync();

		return review;
	}
}
