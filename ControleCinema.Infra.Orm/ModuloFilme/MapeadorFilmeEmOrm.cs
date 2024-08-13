using ControleCinema.Dominio.ModuloFilme;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleCinema.Infra.Orm.ModuloFilme;

public class MapeadorFilmeEmOrm : IEntityTypeConfiguration<Filme>
{
    public void Configure(EntityTypeBuilder<Filme> fBuilder)
    {
        fBuilder.ToTable("TBFilme");

        fBuilder.Property(f => f.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        fBuilder.Property(f => f.Titulo)
            .IsRequired()
            .HasColumnType("varchar(200)");

        fBuilder.Property(f => f.Duracao)
            .IsRequired()
            .HasColumnType("int");

        fBuilder.Property(f => f.Lancamento)
            .IsRequired()
            .HasColumnType("bit");

        fBuilder.HasOne(f => f.Genero)
            .WithMany()
            .HasForeignKey("Genero_Id")
            .HasConstraintName("FK_TBFilme_TBGenero")
            .OnDelete(DeleteBehavior.Restrict);

        fBuilder.Property(s => s.UsuarioId)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("Usuario_Id");

        fBuilder.HasOne(g => g.Usuario)
            .WithMany()
            .HasForeignKey(s => s.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private object[] ObterRegistrosPadrao()
    {
        return
        [
            new
            {
                Id = 1,
                Titulo = "Aladdin",
                Duracao = 90,
                Lancamento = false,
                Genero_Id = 2
            },
            new
            {
                Id = 2,
                Titulo = "Wolverine vs. Deadpool",
                Duracao = 127,
                Lancamento = true,
                Genero_Id = 1
            },
        ];
    }
}