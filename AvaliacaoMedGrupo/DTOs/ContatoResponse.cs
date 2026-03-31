using AvaliacaoMedGrupo.Enums;

namespace AvaliacaoMedGrupo.DTOs;

public class ContatoResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public Sexo Sexo { get; set; }
    public int Idade { get; set; }
    public bool Ativo { get; set; }
}
