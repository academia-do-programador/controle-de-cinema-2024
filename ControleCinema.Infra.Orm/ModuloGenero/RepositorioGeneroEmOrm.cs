using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace ControleCinema.Infra.Orm.ModuloGenero;

public class RepositorioGeneroEmOrm :
    RepositorioBaseEmOrm<Genero>, IRepositorioGenero
{
    public RepositorioGeneroEmOrm(
        ControleCinemaDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Genero> ObterRegistros()
    {
        return _dbContext.Generos;
    }

    public List<Genero> Filtrar(Func<Genero, bool> predicate)
    {
        return _dbContext.Generos
            .Where(predicate)
            .ToList();
    }
}