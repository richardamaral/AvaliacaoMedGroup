using AvaliacaoMedGrupo.DTOs;

namespace AvaliacaoMedGrupo.Services;

public interface IContatoService
{
    Task<List<ContatoResponse>> ObterTodosAtivosAsync();
    Task<ContatoResponse?> ObterPorIdAsync(Guid id);
    Task<ContatoResponse> CriarAsync(CriarContatoRequest request);
    Task<ContatoResponse?> AtualizarAsync(Guid id, AtualizarContatoRequest request);
    Task<bool> DesativarAsync(Guid id);
    Task<bool> ExcluirAsync(Guid id);
}
