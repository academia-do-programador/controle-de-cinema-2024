using ControleCinema.Dominio.ModuloSessao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleCinema.Infra.Orm.ModuloSessao;

public class MapeadorSessaoEmOrm : IEntityTypeConfiguration<Sessao>
{
    public void Configure(EntityTypeBuilder<Sessao> sBuilder)
    {
        sBuilder.ToTable("TBSessao");

        sBuilder.Property(s => s.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        sBuilder.Property(s => s.NumeroMaximoIngressos)
            .IsRequired()
            .HasColumnType("int");

        sBuilder.Property(s => s.Inicio)
            .IsRequired()
            .HasColumnType("datetime2");

        sBuilder.Property(s => s.Encerrada)
            .IsRequired()
            .HasColumnType("bit");

        sBuilder.HasOne(s => s.Filme)
            .WithMany(f => f.Sessoes)
            .HasForeignKey("Filme_Id")
            .OnDelete(DeleteBehavior.Restrict);

        sBuilder.HasOne(s => s.Sala)
            .WithMany()
            .HasForeignKey("Sala_Id")
            .OnDelete(DeleteBehavior.Restrict);

        sBuilder.HasMany(s => s.Ingressos)
            .WithOne(i => i.Sessao)
            .HasForeignKey("Sessao_Id");

        sBuilder.Property(s => s.UsuarioId)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("Usuario_Id");

        sBuilder.HasOne(g => g.Usuario)
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
                Id = 4,
                NumeroMaximoIngressos = 25,
                Inicio = DateTime.Parse("7/08/2024 20:00:00"),
                Encerrada = false,
                Filme_Id = 2,
                Sala_Id = 2
            },
            new
            {
                Id = 3,
                NumeroMaximoIngressos = 35,
                Inicio = DateTime.Parse("10/08/2024 20:00:00"),
                Encerrada = false,
                Filme_Id = 1,
                Sala_Id = 3
            },
            new
            {
                Id = 2,
                NumeroMaximoIngressos = 25,
                Inicio = DateTime.Parse("09/08/2024 17:00:00"),
                Encerrada = false,
                Filme_Id = 1,
                Sala_Id = 3
            },
            new
            {
                Id = 1,
                NumeroMaximoIngressos = 20,
                Inicio = DateTime.Parse("20/07/2024 20:00:00"),
                Encerrada = true,
                Filme_Id = 1,
                Sala_Id = 2
            },
            new
            {
                Id = 6,
                NumeroMaximoIngressos = 30,
                Inicio = DateTime.Parse("02/08/2024 19:30:00"),
                Encerrada = false,
                Filme_Id = 2,
                Sala_Id = 1
            },
            new
            {
                Id = 5,
                NumeroMaximoIngressos = 28,
                Inicio = DateTime.Parse("02/08/2024 19:30:00"),
                Encerrada = true,
                Filme_Id = 2,
                Sala_Id = 2
            }
        ];
    }
}