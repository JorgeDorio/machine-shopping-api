using Machine.Shopping.Api.Models.Customer;
using Machine.Shopping.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Machine.Shopping.Api.Controllers;

[ApiController]
[Route("api/{tenant}/[controller]")]
public class CustomerController(ILogger<CustomerController> logger, CustomersService customersService) : ControllerBase
{
    [HttpPost(Name = "CreateCustomer")]
    public async Task<ActionResult<CreateCustomerResponse>> CreateCustomer([FromBody] CreateCustomerRequest customer,
        [FromRoute] string tenant)
    {
        try
        {
            logger.LogInformation("Cadastrando cliente");
            await customersService.CreateCustomerAsync(customer.ToCustomer(tenant));
            logger.LogInformation("Cliente cadastrado com sucesso");

            var location = Url.Action("CreateCustomer", "Customer");
            return Created(location, new CreateCustomerResponse());
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao cadastrar cliente");

            return BadRequest(e.Message);
        }
    }

    [HttpGet(Name = "GetCustomers")]
    public async Task<ActionResult<CreateCustomerResponse>> GetCustomers(
        [FromRoute] string tenant)
    {
        try
        {
            logger.LogInformation("Buscando cliente");
            var customers = await customersService.GetCustomersAsync(tenant);
            logger.LogInformation("Clientes buscados com sucesso");

            return Ok(customers);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao buscar cliente");

            return BadRequest(e.Message);
        }
    }
}