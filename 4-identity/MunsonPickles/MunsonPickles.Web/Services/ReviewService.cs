using MunsonPickles.Shared.Models;
using MunsonPickles.Shared.Transfer;

namespace MunsonPickles.Web.Services;

public class ReviewService
{
	private readonly HttpClient reviewClient;

	public ReviewService(HttpClient client)
	{
		reviewClient = client;
	}

	public async Task AddReview(string reviewText, List<string> photoUrls, int productId)
	{
		NewReview newReview = new NewReview { 
			PhotoUrls = photoUrls, 	
			ProductId = productId, 
			ReviewText = reviewText 
		};

		await reviewClient.PostAsJsonAsync<NewReview>("/reviews", newReview);
    }

	public async Task<IEnumerable<Review>> GetReviewsForProduct(int productId)
	{
		return await reviewClient.GetFromJsonAsync<IEnumerable<Review>>($"/products/{productId}/reviews");
	}

	public async Task<Review>? GetReviewById(int reviewId)
	{
		return await reviewClient.GetFromJsonAsync<Review>($"/reviews/{reviewId}");
	}

}
