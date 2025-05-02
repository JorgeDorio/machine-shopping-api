using Microsoft.AspNetCore.Identity;

namespace Machine.Shopping.Api.Models.User.Create;

public class CreateUserRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }

    public User ToUser()
    {
        var hashedPassword = new PasswordHasher<CreateUserRequest>().HashPassword(this, Password);
        return new User
        {
            Email = Email,
            PasswordHash = hashedPassword,
            Name = Name
        };
    }
}