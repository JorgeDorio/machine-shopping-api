namespace Machine.Shopping.Api.Models.Professional;

public class CreateProfessionalRequest
{
    public required string Cpf { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }

    public Professional ToProfessional(string tenantId)
    {
        return new Professional()
        {
            Cpf = Cpf,
            Name = Name,
            Phone = Phone,
            TenantId = tenantId
        };
    }
}