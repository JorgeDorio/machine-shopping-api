using Machine.Shopping.Api.Models;
using Machine.Shopping.Api.Models.User;
using Machine.Shopping.Api.Models.User.Create;
using Machine.Shopping.Api.Models.User.Login;
using Machine.Shopping.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Machine.Shopping.Api.Controllers;

[ApiController]
[Route("api/{tenant}/[controller]")]
public class UserController(ILogger<UserController> logger, UsersService usersService) : ControllerBase
{
    [HttpPost("login", Name = "LogIn")]
    public async Task<ActionResult<LoginResponse>> LogIn([FromBody] LoginRequest user)
    {
        try
        {
            logger.LogInformation("LogIn");
            var token = await usersService.LoginAsync(user);
            logger.LogInformation("Usuário autenticado");

            return Ok(new LoginResponse(token));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao logar {Entity}", EntityNames.User);

            return BadRequest();
        }
    }

    [HttpPost("register", Name = "CreateUser")]
    public async Task<ActionResult<CreateUserResponse>> CreateUser([FromBody] CreateUserRequest user,
        [FromRoute] string tenant)
    {
        try
        {
            logger.LogInformation("Cadastrando usuário");
            var token = await usersService.CreateUserAsync(user.ToUser(tenant));
            logger.LogInformation("Usuário cadastrado com sucesso");

            var location = Url.Action("CreateUser", "User");
            return Created(location, new CreateUserResponse(token));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao cadastrar {Entity}", EntityNames.User);

            return BadRequest(e.Message);
        }
    }
}