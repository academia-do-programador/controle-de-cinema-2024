using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace ControleCinema.Infra.Orm.ModuloSessao;

public class RepositorioSessaoEmOrm : IRepositorioSessao
{
    private readonly ControleCinemaDbContext dbContext;

    public RepositorioSessaoEmOrm(ControleCinemaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Inserir(Sessao sessao)
    {
        dbContext.Sessoes.Add(sessao);

        dbContext.SaveChanges();
    }

    public void Editar(Sessao sessao)
    {
        dbContext.Sessoes.Update(sessao);

        dbContext.SaveChanges();
    }

    public void Excluir(Sessao sessao)
    {
        var ingressosParaRemover = sessao.Ingressos;

        dbContext.Ingressos.RemoveRange(ingressosParaRemover);

        dbContext.Sessoes.Remove(sessao);

        dbContext.SaveChanges();
    }

    public Sessao? SelecionarPorId(int id)
    {
        return dbContext.Sessoes
            .Include(s => s.Filme)
            .Include(s => s.Sala)
            .Include(s => s.Ingressos)
            .FirstOrDefault(s => s.Id == id);
    }

    public List<Sessao> SelecionarTodos()
    {
        return dbContext.Sessoes
            .Include(s => s.Filme)
            .Include(s => s.Sala)
            .AsNoTracking()
            .ToList();
    }

    public List<Sessao> Filtrar(Func<Sessao, bool> predicate)
    {
        return dbContext.Sessoes
            .Include(s => s.Filme)
            .Include(s => s.Sala)
            .AsNoTracking().AsEnumerable()
            .Where(predicate)
            .ToList();
    }

    public List<IGrouping<string, Sessao>> ObterSessoesAgrupadasPorFilme(int usuarioId)
    {
        return dbContext.Sessoes
            .Where(s => s.UsuarioId == usuarioId)
            .Include(s => s.Filme)
            .ThenInclude(f => f.Genero)
            .Include(s => s.Sala)
            .Include(s => s.Ingressos)
            .GroupBy(s => s.Filme.Titulo)
            .AsNoTracking()
            .ToList();
    }

    public List<IGrouping<string, Sessao>> ObterSessoesAgrupadasPorFilme()
    {
        return dbContext.Sessoes
            .Where(s => !s.Encerrada)
            .Include(s => s.Filme)
            .ThenInclude(f => f.Genero)
            .Include(s => s.Sala)
            .Include(s => s.Ingressos)
            .GroupBy(s => s.Filme.Titulo)
            .AsNoTracking()
            .ToList();
    }

    public List<Ingresso> SelecionarTodosIngressos(int usuarioSessaoId)
    {
        return dbContext.Ingressos
            .Include(i => i.Sessao)
            .Where(i => i.Sessao.UsuarioId == usuarioSessaoId)
            .ToList();
    }

    public List<Ingresso> SelecionarTodosIngressos()
    {
        return dbContext.Ingressos
            .ToList();
    }

    public List<int> ObterNumerosAssentosOcupados(int sessaoId)
    {
        return dbContext.Ingressos
            .Where(s => s.Id == sessaoId)
            .Include(s => s.Sessao)
            .Select(s => s.NumeroAssento)
            .ToList();
    }
}