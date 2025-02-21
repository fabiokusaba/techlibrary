namespace TechLibrary.Communication.Requests;

public class BooksFilterRequest
{
    public int PageNumber { get; set; }
    public string? Title { get; set; }
}