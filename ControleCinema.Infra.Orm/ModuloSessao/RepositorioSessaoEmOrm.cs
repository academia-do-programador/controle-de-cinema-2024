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

    public List<IGrouping<string, Sessao>> ObterSessoesAgrupadasPorFilme()
    {
        return dbContext.Sessoes
            .Include(s => s.Filme)
            .ThenInclude(f => f.Genero)
            .Include(s => s.Sala)
            .Include(s => s.Ingressos)
            .GroupBy(s => s.Filme.Titulo)
            .AsNoTracking()
            .ToList();
    }
}