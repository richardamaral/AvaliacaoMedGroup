namespace AvaliacaoMedGrupo.Entities;

// tabela de apoio que guarda os tipos de sexo no banco
// assim fica claro no banco que 1 = Masculino, 2 = Feminino e 3 = Nao Especificado
public class SexoTipo
{
    public int SexoTipoId { get; private set; }
    public string Descricao { get; private set; }

    protected SexoTipo()
    {
        Descricao = string.Empty;
    }

    public SexoTipo(int sexoTipoId, string descricao)
    {
        SexoTipoId = sexoTipoId;
        Descricao = descricao;
    }
}
