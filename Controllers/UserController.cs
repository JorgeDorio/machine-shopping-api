using Machine.Shopping.Api.Models.User;
using Machine.Shopping.Api.Models.User.Create;
using Machine.Shopping.Api.Models.User.Login;
using Machine.Shopping.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Machine.Shopping.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(ILogger<UserController> logger, UsersService usersService) : ControllerBase
{
    [HttpGet(Name = "GetUsers")]
    public IEnumerable<User> Get()
    {
        logger.LogInformation("Get all users");
        throw new NotImplementedException();
    }

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
            logger.LogError(e.Message);
            return BadRequest();
        }
    }

    [HttpPost("register", Name = "CreateUser")]
    public async Task<ActionResult<CreateUserResponse>> CreateUser([FromBody] CreateUserRequest user)
    {
        try
        {
            logger.LogInformation("Cadastrando usuário");
            var token = await usersService.CreateAsync(user);
            logger.LogInformation("Usuário cadastrado com sucesso");

            var location = Url.Action("CreateUser", "User");
            return Created(location, new CreateUserResponse(token));
        }
        catch (Exception e)
        {
            logger.LogError("Erro ao cadastrar usuário: " + e.Message);
            return BadRequest(e.Message);
        }
    }
}