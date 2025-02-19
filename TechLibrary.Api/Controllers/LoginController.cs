using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Login.DoLogin;
using TechLibrary.Communication.Responses;
using LoginRequest = TechLibrary.Communication.Requests.LoginRequest;

namespace TechLibrary.Api.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(UserRegisteredResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorMessagesResponse), StatusCodes.Status401Unauthorized)]
    public IActionResult DoLogin(LoginRequest request)
    {
        var useCase = new DoLoginUseCase();
        
        var response = useCase.Execute(request);
        
        return Ok(response);
    }
}
