﻿using ControleCinema.Dominio.Compartilhado;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace ControleCinema.Infra.Orm.ModuloSala;

public class RepositorioSalaEmOrm :
    RepositorioBaseEmOrm<Sala>, IRepositorioSala
{
    public RepositorioSalaEmOrm(
        ControleCinemaDbContext dbContext) : base(dbContext) { }

    protected override DbSet<Sala> ObterRegistros()
    {
        return _dbContext.Salas;
    }
}