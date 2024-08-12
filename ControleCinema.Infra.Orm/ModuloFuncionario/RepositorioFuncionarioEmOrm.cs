using ControleCinema.Dominio.ModuloFuncionario;
using ControleCinema.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace ControleCinema.Infra.Orm.ModuloFuncionario;

public class RepositorioFuncionarioEmOrm :
    RepositorioBaseEmOrm<Funcionario>, IRepositorioFuncionario
{
    public RepositorioFuncionarioEmOrm(
        ControleCinemaDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Funcionario> ObterRegistros()
    {
        return _dbContext.Funcionarios;
    }

    public List<Funcionario> Filtrar(Func<Funcionario, bool> predicate)
    {
        return _dbContext.Funcionarios
            .Where(predicate)
            .ToList();
    }
}