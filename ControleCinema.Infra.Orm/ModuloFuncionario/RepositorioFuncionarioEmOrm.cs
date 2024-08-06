using ControleCinema.Dominio.Compartilhado;
using ControleCinema.Dominio.ModuloFuncionario;
using ControleCinema.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace ControleCinema.Infra.Orm.ModuloFuncionario;

public class RepositorioFuncionarioEmOrm :
    RepositorioBaseEmOrm<Funcionario>, IRepositorio<Funcionario>
{
    public RepositorioFuncionarioEmOrm(
        ControleCinemaDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Funcionario> ObterRegistros()
    {
        return _dbContext.Funcionarios;
    }
}