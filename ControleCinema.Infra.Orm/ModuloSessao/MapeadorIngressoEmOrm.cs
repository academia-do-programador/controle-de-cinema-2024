using ControleCinema.Dominio.ModuloSessao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleCinema.Infra.Orm.ModuloSessao;

public class MapeadorIngressoEmOrm : IEntityTypeConfiguration<Ingresso>
{
    public void Configure(EntityTypeBuilder<Ingresso> iBuilder)
    {
        iBuilder.ToTable("TBIngresso");

        iBuilder.Property(s => s.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        iBuilder.Property(i => i.NumeroAssento)
            .IsRequired()
            .HasColumnType("int");

        iBuilder.Property(i => i.MeiaEntrada)
            .IsRequired()
            .HasColumnType("bit");

        iBuilder.Property(s => s.UsuarioId)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("Usuario_Id");

        iBuilder.HasOne(g => g.Usuario)
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
                NumeroAssento = 10,
                MeiaEntrada =  false,
                Sessao_Id = 1
            },
            new
            {
                Id = 2,
                NumeroAssento = 25,
                MeiaEntrada =  true,
                Sessao_Id = 1
            },
            new
            {
                Id = 3,
                NumeroAssento = 30,
                MeiaEntrada =  false,
                Sessao_Id = 2
            }
        ];
    }
}