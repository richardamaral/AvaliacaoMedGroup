using AvaliacaoMedGrupo.Entities;
using AvaliacaoMedGrupo.Enums;

namespace AvaliacaoMedGrupo.Tests;

public class ContatoEntityTests
{
    private static DateTime CriarDataNascimentoValida()
    {
        return DateTime.Today.AddYears(-30);
    }

    [Theory]
    [InlineData("Maria Silva", Sexo.Feminino)]
    [InlineData("Joao Santos", Sexo.Masculino)]
    public void Construtor_DeveInicializarPropriedadesCorretamente(string nome, Sexo sexo)
    {
        var dataNascimento = CriarDataNascimentoValida();

        var contato = new Contato(nome, dataNascimento, sexo);

        Assert.Equal(nome, contato.Nome);
        Assert.Equal(dataNascimento, contato.DataNascimento);
        Assert.Equal((int)sexo, contato.SexoTipoId);
        Assert.True(contato.Ativo);
        Assert.NotEqual(Guid.Empty, contato.Id);
    }

    [Theory]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(20)]
    public void Idade_DeveSerCalculadaEmTempoDeExecucao(int idadeEsperada)
    {
        var dataNascimento = new DateTime(DateTime.Today.Year - idadeEsperada, 1, 1);

        var contato = new Contato("Teste Idade", dataNascimento, Sexo.Masculino);

        Assert.Equal(idadeEsperada, contato.Idade);
    }

    [Fact]
    public void Idade_DeveConsiderarAniversarioQueAindaNaoOcorreu()
    {
        var hoje = DateTime.Today;
        var idadeBase = 25;
        var dataNascimento = new DateTime(hoje.Year - idadeBase, hoje.Month, hoje.Day).AddDays(1);

        var contato = new Contato("Ana Costa", dataNascimento, Sexo.Feminino);

        Assert.Equal(idadeBase - 1, contato.Idade);
    }

    [Theory]
    [InlineData("Nome Atualizado", 1985, 6, 15, Sexo.Feminino)]
    [InlineData("Carlos Oliveira", 2000, 12, 1, Sexo.Masculino)]
    [InlineData("Ana Souza", 1978, 3, 25, Sexo.NaoEspecificado)]
    public void Atualizar_DeveModificarPropriedades(string novoNome, int ano, int mes, int dia, Sexo novoSexo)
    {
        var contato = new Contato("Nome Original", CriarDataNascimentoValida(), Sexo.Masculino);
        var novaDataNascimento = new DateTime(ano, mes, dia);

        contato.Atualizar(novoNome, novaDataNascimento, novoSexo);

        Assert.Equal(novoNome, contato.Nome);
        Assert.Equal(novaDataNascimento, contato.DataNascimento);
        Assert.Equal((int)novoSexo, contato.SexoTipoId);
    }

    [Fact]
    public void Desativar_DeveMarcarComoInativo()
    {
        var contato = new Contato("Pedro Lima", CriarDataNascimentoValida(), Sexo.Masculino);

        contato.Desativar();

        Assert.False(contato.Ativo);
    }
}
