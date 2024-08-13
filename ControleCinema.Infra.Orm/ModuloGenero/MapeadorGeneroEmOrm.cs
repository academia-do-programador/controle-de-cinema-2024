using ControleCinema.Dominio.ModuloGenero;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleCinema.Infra.Orm.ModuloGenero;

public class MapeadorGeneroEmOrm : IEntityTypeConfiguration<Genero>
{
    public void Configure(EntityTypeBuilder<Genero> eBuilder)
    {
        eBuilder.ToTable("TBGenero");

        eBuilder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        eBuilder.Property(e => e.Descricao)
            .IsRequired()
            .HasColumnType("varchar(100)");

        eBuilder.Property(s => s.UsuarioId)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("Usuario_Id");

        eBuilder.HasOne(g => g.Usuario)
            .WithMany()
            .HasForeignKey(s => s.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private Genero[] ObterRegistrosPadrao()
    {
        return
        [
            new Genero { Id = 1, Descricao = "Ação" },
            new Genero { Id = 2, Descricao = "Animação" },
            new Genero { Id = 3, Descricao = "Aventura" },
            new Genero { Id = 4, Descricao = "Comédia" },
            new Genero { Id = 5, Descricao = "Romance" },
            new Genero { Id = 6, Descricao = "Terror" }
        ];
    }
}