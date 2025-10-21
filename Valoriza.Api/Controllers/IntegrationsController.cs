using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Valoriza.Application.Services;


namespace Valoriza.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class IntegrationsController : ControllerBase
{
    private readonly IntegrationService _svc;


    public IntegrationsController(IntegrationService svc) { _svc = svc; }


    [HttpGet("cep/{cep}")]
    [SwaggerOperation(Summary = "Consulta CEP via BrasilAPI")]
    public async Task<IActionResult> Cep(string cep)
    => Ok(await _svc.CepLookupAsync(cep));


    [HttpPost("explain")]
    [SwaggerOperation(Summary = "Explica um gasto usando OpenAI (ChatGPT)")]
    public async Task<IActionResult> Explain([FromBody] string description)
    => Ok(await _svc.ExplainSpendingWithOpenAIAsync(description));
}