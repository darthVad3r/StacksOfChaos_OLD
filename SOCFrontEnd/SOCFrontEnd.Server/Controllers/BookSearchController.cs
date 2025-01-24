using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Services;
using Newtonsoft.Json;
using SOCDataManager.Models;
using System.Net.Http.Headers;

namespace SOCDataManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookSearchController : ControllerBase
    {
        private readonly ILogger<BookSearchController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BookSearchController(ILogger<BookSearchController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "SearchByAuthor")]
        public async Task<IActionResult> SearchByAuthor(string authorName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(authorName))
                {
                    _logger.LogWarning("Author name is null or whitespace.");
                    return BadRequest("Author name cannot be null or whitespace.");
                }

                using var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "SOCFrontEnd");
                client.BaseAddress = new Uri("https://openlibrary.org/");

                var response = await client.GetAsync($"search.json?q={authorName}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var author = JsonConvert.DeserializeObject<Author>(jsonResponse);

                if (author == null || author.Docs == null || author.Docs.Length == 0)
                {
                    _logger.LogWarning("Author not found");
                    return NotFound("Author not found");
                }

                Console.WriteLine(author.Docs[0].AuthorName);

                return Ok(author.Docs[0].AuthorName);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for books by author.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing your request. {ex.Message}");
            }
        }
    }
}
