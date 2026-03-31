using AvaliacaoMedGrupo.Data;
using AvaliacaoMedGrupo.Entities;
using Microsoft.EntityFrameworkCore;

namespace AvaliacaoMedGrupo.Repositories;

// repository eh responsavel so por acessar o banco de dados
// nao tem regra de negocio aqui, isso fica no service
public class ContatoRepository : IContatoRepository
{
    private readonly AvaliacaoDbContext _context;

    public ContatoRepository(AvaliacaoDbContext context)
    {
        _context = context;
    }

    // busca todos os contatos que estao ativos, ordenados por nome
    public async Task<List<Contato>> ObterTodosAtivosAsync()
    {
        return await _context.Contatos
            .Where(c => c.Ativo)
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<Contato?> ObterPorIdAsync(Guid id)
    {
        return await _context.Contatos.FindAsync(id);
    }

    public async Task AdicionarAsync(Contato contato)
    {
        await _context.Contatos.AddAsync(contato);
    }

    public void Atualizar(Contato contato)
    {
        _context.Contatos.Update(contato);
    }

    public void Remover(Contato contato)
    {
        _context.Contatos.Remove(contato);
    }

    // salva tudo que foi alterado no contexto de uma vez so
    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
