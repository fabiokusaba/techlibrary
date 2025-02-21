using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.UseCases.Books.Filter;

public class FilterBookUseCase
{
    private const int PAGE_SIZE = 10;
    
    public BooksResponse Execute(BooksFilterRequest request)
    {
        var dbContext = new TechLibraryDbContext();
        
        var skip = (request.PageNumber - 1) * PAGE_SIZE;

        var query = dbContext.Books.AsQueryable();

        // Verifica se o título é nulo, se é uma string vazia ou um amontoado de espaços em branco
        if (string.IsNullOrWhiteSpace(request.Title) == false)
            query = query.Where(book => book.Title.Contains(request.Title));

        var books = query
            .OrderBy(book => book.Title).ThenBy(book => book.Author)
            .Skip(skip)
            .Take(PAGE_SIZE)
            .ToList();

        var totalCount = 0;

        if (string.IsNullOrWhiteSpace(request.Title))
            totalCount = dbContext.Books.Count();
        else
            totalCount = dbContext.Books.Count(book => book.Title.Contains(request.Title));

        return new BooksResponse
        {
            Pagination = new PaginationResponse
            {
                PageNumber = request.PageNumber,
                TotalCount = totalCount,
            },
            Books = books.Select(book => new BookResponse
            {
                Id = book.Id,
                Author = book.Author,
                Title = book.Title,
            }).ToList()
        };
    }
}