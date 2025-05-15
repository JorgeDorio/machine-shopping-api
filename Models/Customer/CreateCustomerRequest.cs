namespace Machine.Shopping.Api.Models.Customer;

public class CreateCustomerRequest
{
    public required string Cpf { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }

    public Customer ToCustomer(string tenantId)
    {
        return new Customer()
        {
            Cpf = Cpf,
            Name = Name,
            Phone = Phone,
            TenantId = tenantId
        };
    }
}