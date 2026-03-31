using AvaliacaoMedGrupo.DTOs;
using AvaliacaoMedGrupo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AvaliacaoMedGrupo.Controllers;

// controller so recebe a requisicao e repassa pro service
// a logica de negocio fica toda no service, aqui e so a camada de apresentacao
[ApiController]
[Route("api/[controller]")]
public class ContatosController : ControllerBase
{
    private readonly IContatoService _contatoService;

    public ContatosController(IContatoService contatoService)
    {
        _contatoService = contatoService;
    }

    // lista todos os contatos ativos
    [HttpGet]
    public async Task<ActionResult<List<ContatoResponse>>> ObterTodos()
    {
        var contatos = await _contatoService.ObterTodosAtivosAsync();
        return Ok(contatos);
    }

    // busca um contato especifico pelo id
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ContatoResponse>> ObterPorId(Guid id)
    {
        var contato = await _contatoService.ObterPorIdAsync(id);

        if (contato is null)
            return NotFound();

        return Ok(contato);
    }

    // cria um contato novo e retorna 201 com a localizacao do recurso criado
    [HttpPost]
    public async Task<ActionResult<ContatoResponse>> Criar([FromBody] CriarContatoRequest request)
    {
        try
        {
            var contato = await _contatoService.CriarAsync(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = contato.Id }, contato);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // atualiza os dados de um contato existente
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ContatoResponse>> Atualizar(Guid id, [FromBody] AtualizarContatoRequest request)
    {
        try
        {
            var contato = await _contatoService.AtualizarAsync(id, request);

            if (contato is null)
                return NotFound();

            return Ok(contato);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // desativa o contato sem apagar do banco
    [HttpPatch("{id:guid}/desativar")]
    public async Task<ActionResult> Desativar(Guid id)
    {
        var desativado = await _contatoService.DesativarAsync(id);

        if (!desativado)
            return NotFound();

        return NoContent();
    }

    // exclui o contato do banco
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Excluir(Guid id)
    {
        var excluido = await _contatoService.ExcluirAsync(id);

        if (!excluido)
            return NotFound();

        return NoContent();
    }
}
