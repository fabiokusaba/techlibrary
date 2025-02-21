using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Books.Filter;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet("Filter")]
    [ProducesResponseType(typeof(BooksResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Filter(int pageNumber, string? title)
    {
        var useCase = new FilterBookUseCase();

        var request = new BooksFilterRequest
        {
            PageNumber = pageNumber,
            Title = title
        };

        var result = useCase.Execute(request);

        if (result.Books.Count > 0)
            return Ok(result);
        
        return NoContent();
    }
}
