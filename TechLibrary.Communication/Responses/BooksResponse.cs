namespace TechLibrary.Communication.Responses;

public class BooksResponse
{
    public PaginationResponse Pagination { get; set; } = default!;
    public List<BookResponse> Books { get; set; } = [];
}