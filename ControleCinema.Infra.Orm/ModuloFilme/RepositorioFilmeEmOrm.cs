using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace ControleCinema.Infra.Orm.ModuloFilme;

public class RepositorioFilmeEmOrm :
    RepositorioBaseEmOrm<Filme>, IRepositorioFilme
{
    public RepositorioFilmeEmOrm(
        ControleCinemaDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Filme> ObterRegistros()
    {
        return _dbContext.Filmes;
    }

    public override Filme? SelecionarPorId(int id)
    {
        return ObterRegistros().Include(f => f.Genero)
            .FirstOrDefault(f => f.Id == id);
    }

    public override List<Filme> SelecionarTodos()
    {
        return ObterRegistros().Include(f => f.Genero)
            .ToList();
    }

    public List<Filme> Filtrar(Func<Filme, bool> predicate)
    {
        return ObterRegistros()
            .Include(f => f.Genero)
            .Where(predicate)
            .ToList();
    }
}