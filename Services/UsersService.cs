using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Machine.Shopping.Api.Exceptions;
using Machine.Shopping.Api.Models;
using Machine.Shopping.Api.Models.Customer.Tenant;
using Machine.Shopping.Api.Models.User;
using Machine.Shopping.Api.Models.User.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Machine.Shopping.Api.Services;

public class UsersService
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly IMongoCollection<Tenant> _tenantsCollection;
    private readonly IConfiguration _config;

    public UsersService(IOptions<DatabaseSettings> databaseSettings, IConfiguration config)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _usersCollection = mongoDatabase.GetCollection<User>(databaseSettings.Value.UsersCollectionName);
        _tenantsCollection = mongoDatabase.GetCollection<Tenant>(databaseSettings.Value.TenantsCollectionName);
        _config = config;
    }

    public async Task<string> CreateUserAsync(User user)
    {
        var tenant = await (await _tenantsCollection.FindAsync(x => x.Alias == user.TenantId)).FirstOrDefaultAsync();
        if (tenant is null) throw new NotFoundException("Tenant");
        user.TenantId = tenant.Id;
        if (await (await _usersCollection.FindAsync(x => x.Email == user.Email)).AnyAsync())
            throw new AlreadyExistsException(EntityNames.User);
        await _usersCollection.InsertOneAsync(user);

        var token = CreateToken(user);

        return token;
    }

    public async Task<string> LoginAsync(LoginRequest dto)
    {
        var user = await _usersCollection.Find(x => x.Email == dto.Email).FirstOrDefaultAsync();
        if (user == null) throw new NotFoundException(EntityNames.User);
        var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed) throw new UnauthorizedException("E-mail ou senha inválidos");

        var token = CreateToken(user);

        return token;
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new("tenant", user.TenantId)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("AppSettings:Token")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(issuer: _config.GetValue<string>("AppSettings:Issuer"),
            audience: _config.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}