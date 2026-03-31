using AvaliacaoMedGrupo.DTOs;
using AvaliacaoMedGrupo.Entities;
using AvaliacaoMedGrupo.Enums;
using AvaliacaoMedGrupo.Repositories;

namespace AvaliacaoMedGrupo.Services;

public class ContatoService : IContatoService
{
    // idade minima pra cadastrar um contato, conforme regra de negocio
    private const int IdadeMinima = 18;
    private const int IdadeInvalida = 0;

    private readonly IContatoRepository _contatoRepository;

    public ContatoService(IContatoRepository contatoRepository)
    {
        _contatoRepository = contatoRepository;
    }

    // retorna so os contatos ativos, os inativos nao aparecem na listagem
    public async Task<List<ContatoResponse>> ObterTodosAtivosAsync()
    {
        var contatos = await _contatoRepository.ObterTodosAtivosAsync();
        return contatos.Select(MapearParaResponse).ToList();
    }

    // busca um contato pelo id, mas so retorna se ele estiver ativo
    public async Task<ContatoResponse?> ObterPorIdAsync(Guid id)
    {
        var contato = await _contatoRepository.ObterPorIdAsync(id);

        if (contato is null || !contato.Ativo)
            return null;

        return MapearParaResponse(contato);
    }

    // cria um contato novo, primeiro valida as regras e depois salva no banco
    public async Task<ContatoResponse> CriarAsync(CriarContatoRequest request)
    {
        ValidarDadosContato(request.Nome, request.DataNascimento);

        var contato = new Contato(request.Nome, request.DataNascimento, request.Sexo);

        await _contatoRepository.AdicionarAsync(contato);
        await _contatoRepository.SalvarAlteracoesAsync();

        return MapearParaResponse(contato);
    }

    // atualiza um contato existente, so se ele estiver ativo
    public async Task<ContatoResponse?> AtualizarAsync(Guid id, AtualizarContatoRequest request)
    {
        var contato = await _contatoRepository.ObterPorIdAsync(id);

        if (contato is null || !contato.Ativo)
            return null;

        ValidarDadosContato(request.Nome, request.DataNascimento);

        contato.Atualizar(request.Nome, request.DataNascimento, request.Sexo);
        _contatoRepository.Atualizar(contato);
        await _contatoRepository.SalvarAlteracoesAsync();

        return MapearParaResponse(contato);
    }

    // desativa o contato, eh tipo um "soft delete"
    // o registro continua no banco mas nao aparece mais pra ninguem
    public async Task<bool> DesativarAsync(Guid id)
    {
        var contato = await _contatoRepository.ObterPorIdAsync(id);

        if (contato is null || !contato.Ativo)
            return false;

        contato.Desativar();
        _contatoRepository.Atualizar(contato);
        await _contatoRepository.SalvarAlteracoesAsync();

        return true;
    }

    // esse aqui remove de verdade do banco, diferente do desativar
    public async Task<bool> ExcluirAsync(Guid id)
    {
        var contato = await _contatoRepository.ObterPorIdAsync(id);

        if (contato is null)
            return false;

        _contatoRepository.Remover(contato);
        await _contatoRepository.SalvarAlteracoesAsync();

        return true;
    }

    // aqui ficam todas as validacoes das regras de negocio do contato
    private static void ValidarDadosContato(string nome, DateTime dataNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do contato e obrigatorio.");

        // verificacao data de nascimento nao pode ser maior que a data de hoje
        if (dataNascimento > DateTime.Today)
            throw new ArgumentException("A data de nascimento nao pode ser maior que a data de hoje.");

        var idade = CalcularIdade(dataNascimento);

        // se a idade retornar zero
        if (idade == IdadeInvalida)
            throw new ArgumentException("A idade nao pode ser igual a 0.");

    }

    // calcula a idade baseado na data de nascimento
    // leva em conta se a pessoa ja fez aniversario no ano atual ou nao
    private static int CalcularIdade(DateTime dataNascimento)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - dataNascimento.Year;

        if (dataNascimento.Date > hoje.AddYears(-idade))
            idade--;

        return idade;
    }

    // converte a entidade Contato pro DTO de resposta que vai pro cliente
    private static ContatoResponse MapearParaResponse(Contato contato)
    {
        return new ContatoResponse
        {
            Id = contato.Id,
            Nome = contato.Nome,
            DataNascimento = contato.DataNascimento,
            Sexo = (Sexo)contato.SexoTipoId,
            Idade = contato.Idade,
            Ativo = contato.Ativo
        };
    }
}
