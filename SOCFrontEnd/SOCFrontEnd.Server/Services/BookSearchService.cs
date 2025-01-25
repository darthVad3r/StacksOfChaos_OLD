using Newtonsoft.Json;
using SOCDataManager.Models;
using System.Net.Http.Headers;

namespace SOCDataManager.Services
{
    public class BookSearchService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BookSearchService> _logger;

        public BookSearchService(IHttpClientFactory httpClientFactory, ILogger<BookSearchService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Author?> GetAuthorDetailsAsync(string authorName)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "SOCFrontEnd");
                client.BaseAddress = new Uri("https://openlibrary.org/");

                var response = await client.GetAsync($"search.json?q={authorName}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var author = JsonConvert.DeserializeObject<Author>(jsonResponse);

                return author;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while querying the OpenLibraryAPI.");
                return null;
            }
        }
    }
}