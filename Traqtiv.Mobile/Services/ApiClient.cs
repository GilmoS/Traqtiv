using System.Net.Http.Headers;
using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Models;

namespace Traqtiv.Mobile.Services;

// ApiClient is responsible for managing the HTTP client and attaching the authentication token for API requests.
// It uses the SmartFitnessClient to interact with the API and ensures that the authentication token is included in the request headers for authorized access.
public class ApiClient
{
    private readonly SmartFitnessClient _client;
    private readonly HttpClient _httpClient;

    // Constructor that initializes the HTTP client and the SmartFitnessClient instance.
    public ApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TraqtivApi");
        _client = new SmartFitnessClient(AppConstants.ApiBaseUrl, _httpClient);
    }

    // Attaches the authentication token to the HTTP client's default request headers.
    public async Task AttachTokenAsync()
    {
        var token = await SecureStorageHelper.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    // Exposes the SmartFitnessClient instance for making API calls.
    public SmartFitnessClient Client => _client;
}