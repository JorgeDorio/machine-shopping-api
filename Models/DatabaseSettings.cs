namespace Machine.Shopping.Api.Models;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string UsersCollectionName { get; set; } = null!;
    public string CustomersCollectionName { get; set; } = null!;
    public string TenantsCollectionName { get; set; } = null!;
    public string ProfessionalsCollectionName { get; set; } = null!;
}