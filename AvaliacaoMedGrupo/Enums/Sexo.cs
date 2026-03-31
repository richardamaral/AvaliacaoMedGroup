using System.Text.Json.Serialization;

namespace AvaliacaoMedGrupo.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Sexo
{
    Masculino = 1,
    Feminino = 2,
    NaoEspecificado = 3
}
