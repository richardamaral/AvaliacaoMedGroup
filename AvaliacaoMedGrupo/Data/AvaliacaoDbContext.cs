using AvaliacaoMedGrupo.Entities;
using AvaliacaoMedGrupo.Enums;
using Microsoft.EntityFrameworkCore;

namespace AvaliacaoMedGrupo.Data;

public class AvaliacaoDbContext : DbContext
{
    private const int TamanhoMaximoNome = 200;
    private const int TamanhoMaximoDescricaoSexo = 50;

    public DbSet<Contato> Contatos { get; set; }
    public DbSet<SexoTipo> SexoTipo { get; set; }

    public AvaliacaoDbContext(DbContextOptions<AvaliacaoDbContext> options) : base(options) { }

    // aqui eu configuro como as tabelas vao ficar no banco
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contato>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nome).IsRequired().HasMaxLength(TamanhoMaximoNome);
            entity.Property(c => c.DataNascimento).IsRequired();
            entity.Property(c => c.Ativo).IsRequired();

            // idade nao vai pro banco pois eh calculada em tempo de execucao
            entity.Ignore(c => c.Idade);

            // relacionamento com a tabela SexoTipo
            entity.HasOne(c => c.SexoTipo)
                .WithMany()
                .HasForeignKey(c => c.SexoTipoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SexoTipo>(entity =>
        {
            entity.ToTable("SexoTipo");
            entity.HasKey(s => s.SexoTipoId);
            entity.Property(s => s.Descricao).IsRequired().HasMaxLength(TamanhoMaximoDescricaoSexo);

            // ja populo a tabela com os valores do enum pra ficar tudo certinho no banco
            entity.HasData(
                new SexoTipo((int)Sexo.Masculino, nameof(Sexo.Masculino)),
                new SexoTipo((int)Sexo.Feminino, nameof(Sexo.Feminino)),
                new SexoTipo((int)Sexo.NaoEspecificado, "Nao Especificado")
            );
        });
    }
}
