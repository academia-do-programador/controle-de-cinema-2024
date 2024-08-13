using ControleCinema.Dominio.Compartilhado;

namespace ControleCinema.Dominio.ModuloSessao;

public interface IRepositorioSessao : IRepositorio<Sessao>
{
    List<IGrouping<string, Sessao>> ObterSessoesAgrupadasPorFilme(int usuarioId);
    List<IGrouping<string, Sessao>> ObterSessoesAgrupadasPorFilme();
    List<Ingresso> SelecionarTodosIngressos(int usuarioSessaoId);
    List<Ingresso> SelecionarTodosIngressos();
    List<int> ObterNumerosAssentosOcupados(int sessaoId);
}