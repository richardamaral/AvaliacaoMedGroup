using AvaliacaoMedGrupo.Enums;

namespace AvaliacaoMedGrupo.Entities;

public class Contato
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public bool Ativo { get; private set; }

    // chave estrangeira que aponta pra tabela SexoTipo no banco
    // substitui a antiga coluna Sexo, pois o SexoTipoId ja representa o mesmo valor
    public int SexoTipoId { get; private set; }
    public SexoTipo? SexoTipo { get; private set; }

    // a idade nao fica salva no banco, ela eh calculada toda vez que alguem acessa
    // pq se salvasse no banco ia ficar desatualizada todo ano
    public int Idade
    {
        get
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;

            // aqui eu verifico se a pessoa ja fez aniversario esse ano
            // se ainda nao fez, tiro 1 da idade
            if (DataNascimento.Date > hoje.AddYears(-idade))
                idade--;

            return idade;
        }
    }

    // construtor vazio que o entity framework precisa pra funcionar
    protected Contato()
    {
        Nome = string.Empty;
    }

    public Contato(string nome, DateTime dataNascimento, Sexo sexo)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        DataNascimento = dataNascimento;
        SexoTipoId = (int)sexo;
        Ativo = true;
    }

    // metodo pra atualizar os dados do contato sem precisar criar um novo objeto
    public void Atualizar(string nome, DateTime dataNascimento, Sexo sexo)
    {
        Nome = nome;
        DataNascimento = dataNascimento;
        SexoTipoId = (int)sexo;
    }

    // desativar eh diferente de excluir, aqui eu so marco como inativo
    // assim o contato nao aparece mais nas listagens mas continua no banco
    public void Desativar()
    {
        Ativo = false;
    }
}
