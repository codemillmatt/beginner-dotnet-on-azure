using Microsoft.Identity.Web;
using MunsonPickles.Shared.Models;
using MunsonPickles.Shared.Transfer;
using System.Net.Http.Headers;

namespace MunsonPickles.Web.Services;

public class ReviewService
{
	private readonly HttpClient reviewClient;
	private readonly ITokenAcquisition tokenAcquisition;
	private readonly IConfiguration configuration;

    public ReviewService(HttpClient client, ITokenAcquisition token, IConfiguration configure)
	{
		reviewClient = client;
		tokenAcquisition = token;
		configuration = configure;
	}

	public async Task AddReview(string reviewText, List<string> photoUrls, int productId)
	{
		try
		{
			NewReview newReview = new NewReview
			{
				PhotoUrls = photoUrls,
				ProductId = productId,
				ReviewText = reviewText
			};

            var scopes = configuration["ReviewApi:Scopes"]?.Split(' ')!;
			
            string accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);

			reviewClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			await reviewClient.PostAsJsonAsync<NewReview>("/reviews", newReview);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
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
