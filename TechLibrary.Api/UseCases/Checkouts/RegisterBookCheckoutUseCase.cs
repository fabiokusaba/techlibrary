using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts;

public class RegisterBookCheckoutUseCase
{
    private const int MAX_LOAN_DAYS = 7;
    
    private readonly LoggedUserService _loggedUserService;

    public RegisterBookCheckoutUseCase(LoggedUserService loggedUserService)
    {
        _loggedUserService = loggedUserService;
    }
    
    public void Execute(Guid bookId)
    {
        var dbContext = new TechLibraryDbContext();
        
        Validate(dbContext, bookId);

        var user = _loggedUserService.GetLoggedUser(dbContext);

        var entity = new Checkout
        {
            UserId = user.Id,
            BookId = bookId,
            ExpectedReturnDate = DateTime.UtcNow.AddDays(MAX_LOAN_DAYS),
        };

        dbContext.Checkouts.Add(entity);
        
        dbContext.SaveChanges();
    }

    private void Validate(TechLibraryDbContext dbContext, Guid bookId)
    {
        var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);

        if (book is null)
            throw new NotFoundException("Livro não encontrado.");
        
        // Verificando empréstimos ativos
        var amountBookNotReturned = dbContext
            .Checkouts
            .Count(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);

        // Verificando se toda a quantidade de livros que tenho está emprestada
        if (amountBookNotReturned == book.Amount)
            throw new ConflictException("Livro não está disponível para empréstimo.");
    }
}