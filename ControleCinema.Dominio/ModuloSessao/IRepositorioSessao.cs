using ControleCinema.Dominio.Compartilhado;

namespace ControleCinema.Dominio.ModuloSessao;

public interface IRepositorioSessao : IRepositorio<Sessao>
{
    List<IGrouping<string, Sessao>> ObterSessoesAgrupadasPorFilme();
    List<int> ObterNumerosAssentosOcupados(int sessaoId);
}