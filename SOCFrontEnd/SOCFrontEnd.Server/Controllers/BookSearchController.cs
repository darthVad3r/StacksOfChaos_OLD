using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Services;
using Newtonsoft.Json;
using SOCDataManager.Models;
using System.Net.Http.Headers;
using SOCDataManager.Services;

namespace SOCDataManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookSearchController : ControllerBase
    {
        private readonly ILogger<BookSearchController> _logger;
        private readonly BookSearchService _bookSearchService;

        public BookSearchController(ILogger<BookSearchController> logger, BookSearchService bookSearchService)
        {
            _logger = logger;
            _bookSearchService = bookSearchService;
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

                var author = await _bookSearchService.GetAuthorDetailsAsync(authorName);

                if (author == null || author.Docs == null || author.Docs.Length == 0)
                {
                    _logger.LogWarning("Author not found");
                    return NotFound("Author not found");
                }

                var authorNameOutput = author.Docs[0].AuthorName;
                Console.WriteLine(authorNameOutput);

                return Ok(authorNameOutput);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for books by author.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing your request. {ex.Message}");
            }
        }
    }
}