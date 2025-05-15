using Machine.Shopping.Api.Exceptions;
using Machine.Shopping.Api.Models;
using Machine.Shopping.Api.Models.Professional;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Machine.Shopping.Api.Services;

public class ProfessionalsService
{
    private readonly IMongoCollection<Professional> _professionalsCollection;

    public ProfessionalsService(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _professionalsCollection = mongoDatabase.GetCollection<Professional>(databaseSettings.Value.ProfessionalsCollectionName);
    }

    public async Task CreateProfessionalAsync(Professional professional)
    {
        if (await (await _professionalsCollection.FindAsync(x => x.Cpf == professional.Cpf || x.Phone == professional.Phone))
            .AnyAsync())
            throw new AlreadyExistsException(EntityNames.Professional);
        await _professionalsCollection.InsertOneAsync(professional);
    }

    public async Task<IEnumerable<Professional>> GetProfessionalsAsync(string tenant)
    {
        var filter = Builders<Professional>.Filter.Eq(c => c.TenantId, tenant);
        var cursor = await _professionalsCollection.FindAsync(filter);
        return await cursor.ToListAsync();
    }
}