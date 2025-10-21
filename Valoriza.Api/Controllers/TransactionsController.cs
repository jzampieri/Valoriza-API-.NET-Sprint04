using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Valoriza.Application.DTOs;
using Valoriza.Application.Services;


namespace Valoriza.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _service;
    private readonly IMapper _mapper;


    public TransactionsController(TransactionService service, IMapper mapper)
    { _service = service; _mapper = mapper; }


    [HttpPost]
    [SwaggerOperation(Summary = "Cria uma transação", Description = "CRUD - Create")]
    public async Task<ActionResult<TransactionViewDto>> Create([FromBody] TransactionCreateDto dto)
    => Ok(await _service.CreateAsync(dto));


    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Busca transação por Id", Description = "CRUD - Read")]
    public async Task<ActionResult<TransactionViewDto>> Get(Guid id)
    {
        var item = await _service.GetAsync(id);
        return item is null ? NotFound() : Ok(item);
    }


    [HttpGet("user/{userId:guid}")]
    [SwaggerOperation(Summary = "Lista transações do usuário (ordenadas por data desc)")]
    public async Task<ActionResult<IReadOnlyList<TransactionViewDto>>> ListUser(Guid userId)
    => Ok(await _service.ListByUserAsync(userId));


    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Atualiza transação", Description = "CRUD - Update")]
    public async Task<ActionResult<TransactionViewDto>> Update(Guid id, [FromBody] TransactionUpdateDto dto)
    {
        var item = await _service.UpdateAsync(id, dto);
        return item is null ? NotFound() : Ok(item);
    }


    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Remove transação", Description = "CRUD - Delete")]
    public async Task<IActionResult> Delete(Guid id)
    => await _service.DeleteAsync(id) ? NoContent() : NotFound();


    [HttpGet("search")]
    [SwaggerOperation(Summary = "Pesquisa com LINQ", Description = "Filtros: período, categoria, faixa de valor e risco")]
    public async Task<ActionResult<IReadOnlyList<TransactionViewDto>>> Search(
    [FromQuery] Guid userId,
    [FromQuery] DateTime? start,
    [FromQuery] DateTime? end,
    [FromQuery] Domain.Entities.Category? category,
    [FromQuery] decimal? min,
    [FromQuery] decimal? max,
    [FromQuery] bool? risky)
    {
        var result = await _service.SearchAsync(userId, start, end, category, min, max, risky);
        return Ok(result);
    }
}