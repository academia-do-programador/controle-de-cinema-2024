﻿using ControleCinema.Dominio.Compartilhado;

namespace ControleCinema.Dominio.ModuloSessao;

public interface IRepositorioSessao : IRepositorio<Sessao>
{
    List<IGrouping<string, Sessao>> ObterSessoesAgrupadasPorFilme();
    List<IGrouping<string, Sessao>> ObterSessoesDisponiveisAgrupadas();
}