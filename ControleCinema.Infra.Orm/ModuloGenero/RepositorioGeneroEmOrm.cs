using ControleCinema.Dominio.Compartilhado;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace ControleCinema.Infra.Orm.ModuloGenero;

public class RepositorioGeneroEmOrm :
    RepositorioBaseEmOrm<Genero>, IRepositorio<Genero>
{
    public RepositorioGeneroEmOrm(
        ControleCinemaDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Genero> ObterRegistros()
    {
        return _dbContext.Generos;
    }
}