using AvaliacaoMedGrupo.Entities;

namespace AvaliacaoMedGrupo.Repositories;

public interface IContatoRepository
{
    Task<List<Contato>> ObterTodosAtivosAsync();
    Task<Contato?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Contato contato);
    void Atualizar(Contato contato);
    void Remover(Contato contato);
    Task SalvarAlteracoesAsync();
}
