// imports go here

namespace An.Example
{
	public class MyController : Controller
	{
		public async Task<ActionResult> MyRequest(string id)
		{
			var accessToken = GetCurrentUserAccessToken();
			var json = await ApiClient.Fetch<EpisodeVm>($"mediaelement/{id}", accessToken);
			return Json(json);
		}

		private string GetCurrentUserAccessToken()
		{
			var currentUser = IoC.UserProvider().GetCurrentUser();
			return currentUser == null
				? string.Empty
				: currentUser.AccessToken.Raw;
		}
	}

	public class ApiClient
	{
		private static readonly HttpClient Client;
		private static readonly string ApiKey;

		static ApiClient()
		{
			var apiBasePath = IoC.Settings().ApiBaseUrl;
			Client = CreateHttpClient(apiBasePath);
			ApiKey = IoC.Settings().ApiKey;
		}

		public static async Task<T> Fetch<T>(string url, string accessToken) where T : class
		{
			var urlWithApiKey = AddApiKeyToUrl(url);
			var response = await Client.SendAsync(GetRequestMessage(accessToken, urlWithApiKey));
			var warning = response.Headers.Warning as IEnumerable<WarningHeaderValue>;
			if (!response.IsSuccessStatusCode)
			{
				// handle / retry non-success???
				return null;
			}

			var json = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(json);
		}

	    private static string AddApiKeyToUrl(string url)
	    {
			if (url.Contains("?"))
			{
				return url + "&apikey=" + ApiKey;
			}

			return url + "?apikey=" + ApiKey;
      	}

		private static HttpClient CreateHttpClient(string apiBasePath)
		{
			var handler = new WebRequestHandler
			{
				AllowAutoRedirect = true,
				CachePolicy = new RequestCachePolicy(RequestCacheLevel.Default),
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};
			return new HttpClient(handler) { BaseAddress = new Uri(apiBasePath) };
		}

		private static HttpRequestMessage GetRequestMessage(string accessToken, string url)
		{
			var request = new HttpRequestMessage();
			request.Method = HttpMethod.Get;
			request.RequestUri = new Uri(url, UriKind.Relative);
			if (!string.IsNullOrWhiteSpace(accessToken))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			}

			return request;
		}
	}
}
