using AvaliacaoMedGrupo.DTOs;
using AvaliacaoMedGrupo.Entities;
using AvaliacaoMedGrupo.Enums;
using AvaliacaoMedGrupo.Repositories;
using AvaliacaoMedGrupo.Services;
using Moq;

namespace AvaliacaoMedGrupo.Tests;

public class ContatoServiceTests
{
    private readonly Mock<IContatoRepository> _repositoryMock;
    private readonly ContatoService _service;

    public ContatoServiceTests()
    {
        _repositoryMock = new Mock<IContatoRepository>();
        _service = new ContatoService(_repositoryMock.Object);
    }

    // cria uma data de nascimento valida pra usar nos testes sem ficar repetindo
    private static DateTime CriarDataNascimentoValida()
    {
        return DateTime.Today.AddYears(-30);
    }

    [Theory]
    [InlineData("Carlos Souza", Sexo.Masculino)]
    [InlineData("Maria Oliveira", Sexo.Feminino)]
    public async Task CriarAsync_ComDadosValidos_DeveRetornarContatoCriado(string nome, Sexo sexo)
    {
        var request = new CriarContatoRequest
        {
            Nome = nome,
            DataNascimento = CriarDataNascimentoValida(),
            Sexo = sexo
        };

        _repositoryMock.Setup(r => r.AdicionarAsync(It.IsAny<Contato>())).Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.SalvarAlteracoesAsync()).Returns(Task.CompletedTask);

        var resultado = await _service.CriarAsync(request);

        Assert.Equal(request.Nome, resultado.Nome);
        Assert.Equal(request.DataNascimento, resultado.DataNascimento);
        Assert.Equal(request.Sexo, resultado.Sexo);
        Assert.True(resultado.Ativo);
        Assert.True(resultado.Idade > 0);
        _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Contato>()), Times.Once);
        _repositoryMock.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task CriarAsync_ComNomeInvalido_DeveLancarExcecao(string? nome)
    {
        var request = new CriarContatoRequest
        {
            Nome = nome!,
            DataNascimento = CriarDataNascimentoValida(),
            Sexo = Sexo.Masculino
        };

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(request));
        Assert.Equal("O nome do contato e obrigatorio.", excecao.Message);
    }

    [Fact]
    public async Task CriarAsync_ComDataNascimentoFutura_DeveLancarExcecao()
    {
        var request = new CriarContatoRequest
        {
            Nome = "Teste",
            DataNascimento = DateTime.Today.AddDays(1),
            Sexo = Sexo.Feminino
        };

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(request));
        Assert.Equal("A data de nascimento nao pode ser maior que a data de hoje.", excecao.Message);
    }

    [Fact]
    public async Task CriarAsync_ComIdadeZero_DeveLancarExcecao()
    {
        var request = new CriarContatoRequest
        {
            Nome = "Recem Nascido",
            DataNascimento = DateTime.Today,
            Sexo = Sexo.Feminino
        };

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(request));
        Assert.Equal("A idade nao pode ser igual a 0.", excecao.Message);
    }

    [Fact]
    public async Task ObterTodosAtivosAsync_DeveRetornarApenasContatosAtivos()
    {
        var contatos = new List<Contato>
        {
            new Contato("Ativo 1", CriarDataNascimentoValida(), Sexo.Masculino),
            new Contato("Ativo 2", CriarDataNascimentoValida().AddYears(-5), Sexo.Feminino)
        };

        _repositoryMock.Setup(r => r.ObterTodosAtivosAsync()).ReturnsAsync(contatos);

        var resultado = await _service.ObterTodosAtivosAsync();

        Assert.Equal(contatos.Count, resultado.Count);
        Assert.All(resultado, c => Assert.True(c.Ativo));
    }

    [Fact]
    public async Task ObterPorIdAsync_ComContatoAtivoExistente_DeveRetornarContato()
    {
        var contato = new Contato("Existente", CriarDataNascimentoValida(), Sexo.Masculino);

        _repositoryMock.Setup(r => r.ObterPorIdAsync(contato.Id)).ReturnsAsync(contato);

        var resultado = await _service.ObterPorIdAsync(contato.Id);

        Assert.NotNull(resultado);
        Assert.Equal(contato.Nome, resultado!.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComContatoInexistente_DeveRetornarNull()
    {
        _repositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Contato?)null);

        var resultado = await _service.ObterPorIdAsync(Guid.NewGuid());

        Assert.Null(resultado);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComContatoInativo_DeveRetornarNull()
    {
        var contato = new Contato("Inativo", CriarDataNascimentoValida(), Sexo.Masculino);
        contato.Desativar();

        _repositoryMock.Setup(r => r.ObterPorIdAsync(contato.Id)).ReturnsAsync(contato);

        var resultado = await _service.ObterPorIdAsync(contato.Id);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task AtualizarAsync_ComDadosValidos_DeveRetornarContatoAtualizado()
    {
        var contato = new Contato("Original", CriarDataNascimentoValida(), Sexo.Masculino);
        var nomeAtualizado = "Atualizado";
        var novaDataNascimento = CriarDataNascimentoValida().AddYears(-5);
        var request = new AtualizarContatoRequest
        {
            Nome = nomeAtualizado,
            DataNascimento = novaDataNascimento,
            Sexo = Sexo.Feminino
        };

        _repositoryMock.Setup(r => r.ObterPorIdAsync(contato.Id)).ReturnsAsync(contato);
        _repositoryMock.Setup(r => r.SalvarAlteracoesAsync()).Returns(Task.CompletedTask);

        var resultado = await _service.AtualizarAsync(contato.Id, request);

        Assert.NotNull(resultado);
        Assert.Equal(nomeAtualizado, resultado!.Nome);
        Assert.Equal(novaDataNascimento, resultado.DataNascimento);
    }

    [Fact]
    public async Task AtualizarAsync_ComContatoInexistente_DeveRetornarNull()
    {
        _repositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Contato?)null);

        var resultado = await _service.AtualizarAsync(Guid.NewGuid(), new AtualizarContatoRequest
        {
            Nome = "Teste",
            DataNascimento = CriarDataNascimentoValida(),
            Sexo = Sexo.Masculino
        });

        Assert.Null(resultado);
    }

    [Fact]
    public async Task DesativarAsync_ComContatoExistente_DeveRetornarTrue()
    {
        var contato = new Contato("Ativo", CriarDataNascimentoValida(), Sexo.Masculino);

        _repositoryMock.Setup(r => r.ObterPorIdAsync(contato.Id)).ReturnsAsync(contato);
        _repositoryMock.Setup(r => r.SalvarAlteracoesAsync()).Returns(Task.CompletedTask);

        var resultado = await _service.DesativarAsync(contato.Id);

        Assert.True(resultado);
        Assert.False(contato.Ativo);
    }

    [Fact]
    public async Task DesativarAsync_ComContatoInexistente_DeveRetornarFalse()
    {
        _repositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Contato?)null);

        var resultado = await _service.DesativarAsync(Guid.NewGuid());

        Assert.False(resultado);
    }

    [Fact]
    public async Task ExcluirAsync_ComContatoExistente_DeveRetornarTrue()
    {
        var contato = new Contato("Para Excluir", CriarDataNascimentoValida(), Sexo.Masculino);

        _repositoryMock.Setup(r => r.ObterPorIdAsync(contato.Id)).ReturnsAsync(contato);
        _repositoryMock.Setup(r => r.SalvarAlteracoesAsync()).Returns(Task.CompletedTask);

        var resultado = await _service.ExcluirAsync(contato.Id);

        Assert.True(resultado);
        _repositoryMock.Verify(r => r.Remover(contato), Times.Once);
    }

    [Fact]
    public async Task ExcluirAsync_ComContatoInexistente_DeveRetornarFalse()
    {
        _repositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Contato?)null);

        var resultado = await _service.ExcluirAsync(Guid.NewGuid());

        Assert.False(resultado);
    }
}
