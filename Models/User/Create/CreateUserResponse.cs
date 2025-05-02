namespace Machine.Shopping.Api.Models.User.Create;

public class CreateUserResponse(string token)
{
    public string Token { get; set; } = token;
}