using Machine.Shopping.Api.Exceptions;
using Machine.Shopping.Api.Models;
using Machine.Shopping.Api.Models.Customer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Machine.Shopping.Api.Services;

public class CustomersService
{
    private readonly IMongoCollection<Customer> _customersCollection;

    public CustomersService(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _customersCollection = mongoDatabase.GetCollection<Customer>(databaseSettings.Value.CustomersCollectionName);
    }

    public async Task CreateCustomerAsync(Customer customer)
    {
        if (await (await _customersCollection.FindAsync(x => x.Cpf == customer.Cpf || x.Phone == customer.Phone))
            .AnyAsync())
            throw new AlreadyExistsException(EntityNames.Customer);
        await _customersCollection.InsertOneAsync(customer);
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync(string tenantId)
    {
        var filter = Builders<Customer>.Filter.Eq(c => c.TenantId, tenantId);
        var cursor = await _customersCollection.FindAsync(filter);
        return await cursor.ToListAsync();
    }
}