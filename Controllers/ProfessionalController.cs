using Machine.Shopping.Api.Models.Professional;
using Machine.Shopping.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Machine.Shopping.Api.Controllers;

[ApiController]
[Route("api/{tenant}/[controller]")]
public class ProfessionalController(ILogger<ProfessionalController> logger, ProfessionalsService professionalsService) : ControllerBase
{
    [HttpPost(Name = "CreateProfessional")]
    public async Task<ActionResult<CreateProfessionalResponse>> CreateProfessional([FromBody] CreateProfessionalRequest professional,
        [FromRoute] string tenant)
    {
        try
        {
            logger.LogInformation("Cadastrando cliente");
            await professionalsService.CreateProfessionalAsync(professional.ToProfessional(tenant));
            logger.LogInformation("Cliente cadastrado com sucesso");

            var location = Url.Action("CreateProfessional", "Professional");
            return Created(location, new CreateProfessionalResponse());
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao cadastrar cliente");

            return BadRequest(e.Message);
        }
    }

    [HttpGet(Name = "GetProfessionals")]
    public async Task<ActionResult<CreateProfessionalResponse>> GetProfessionals(
        [FromRoute] string tenant)
    {
        try
        {
            logger.LogInformation("Buscando cliente");
            var professionals = await professionalsService.GetProfessionalsAsync(tenant);
            logger.LogInformation("Clientes buscados com sucesso");

            return Ok(professionals);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao buscar cliente");

            return BadRequest(e.Message);
        }
    }
}