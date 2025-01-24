using Microsoft.AspNetCore.Mvc;
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

                var response = await client.GetFromJsonAsync<Author>($"search.json?q={authorName}");

                if (response == null || response.Docs == null || response.Docs.Length == 0)
                {
                    return NotFound("No authors found.");
                }

                return Ok(response.Docs[0].AuthorName);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for books by author.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing your request. {ex.Message}");
            }
        }
    }
}
