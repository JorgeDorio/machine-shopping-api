namespace Machine.Shopping.Api.Models.User.Login;

public class LoginResponse(string token)
{
    public string Token { get; set; } = token;
}