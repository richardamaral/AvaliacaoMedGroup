using AvaliacaoMedGrupo.Enums;

namespace AvaliacaoMedGrupo.DTOs;

public class AtualizarContatoRequest
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public Sexo Sexo { get; set; }
}
