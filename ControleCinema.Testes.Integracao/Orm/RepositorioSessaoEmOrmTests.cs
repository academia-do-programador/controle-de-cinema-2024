using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.Infra.Orm.Compartilhado;
using ControleCinema.Infra.Orm.ModuloFilme;
using ControleCinema.Infra.Orm.ModuloGenero;
using ControleCinema.Infra.Orm.ModuloSala;
using ControleCinema.Infra.Orm.ModuloSessao;

namespace ControleCinema.Testes.Integracao.Orm;

[TestClass]
public class RepositorioSessaoEmOrmTests
{
    private ControleCinemaDbContext db;
    private RepositorioGeneroEmOrm repositorioGenero;
    private RepositorioFilmeEmOrm repositorioFilme;
    private RepositorioSalaEmOrm repositorioSala;
    private RepositorioSessaoEmOrm repositorioSessao;

    [TestInitialize]
    public void ConfigurarTestes()
    {
        db = new ControleCinemaDbContext();
        
        db.Ingressos.RemoveRange(db.Ingressos);
        db.Sessoes.RemoveRange(db.Sessoes);
        db.Salas.RemoveRange(db.Salas);
        db.Filmes.RemoveRange(db.Filmes);
        db.Generos.RemoveRange(db.Generos);

        db.SaveChanges();

        repositorioSessao = new RepositorioSessaoEmOrm(db);
        repositorioSala = new RepositorioSalaEmOrm(db);
        repositorioFilme = new RepositorioFilmeEmOrm(db);
        repositorioGenero = new RepositorioGeneroEmOrm(db);
    }
    
    [TestMethod]
    public void Deve_Inserir_Sessao()
    {
        // Arrange
        var genero = new Genero("Ação");
        repositorioGenero.Inserir(genero);

        var filme = new Filme("Blade Runner 2049", 120, genero, false);
        repositorioFilme.Inserir(filme);

        var sala = new Sala(1, 30);
        repositorioSala.Inserir(sala);

        var inicio = DateTime.Parse("20/08/2024 18:00:00");
        var sessao = new Sessao(filme, sala, 30, inicio);
        
        // Act
        repositorioSessao.Inserir(sessao);

        // Assert
        var sessaoSelecionada = repositorioSessao.SelecionarPorId(sessao.Id);
        
        Assert.IsNotNull(sessaoSelecionada);
        Assert.AreEqual(sessao, sessaoSelecionada);
    }

    [TestMethod]
    public void Deve_Encerrar_Sessao()
    {
        // Arrange
        var genero = new Genero("Ação");
        repositorioGenero.Inserir(genero);

        var filme = new Filme("Blade Runner 2049", 120, genero, false);
        repositorioFilme.Inserir(filme);

        var sala = new Sala(1, 30);
        repositorioSala.Inserir(sala);

        var inicio = DateTime.Parse("20/08/2024 18:00:00");
        
        var sessao = new Sessao(filme, sala, 30, inicio);
        repositorioSessao.Inserir(sessao);
        
        sessao.Encerrar();
        
        // Act
        repositorioSessao.Editar(sessao);
        
        // Assert
        var sessaoSelecionada = repositorioSessao.SelecionarPorId(sessao.Id);

        Assert.IsNotNull(sessaoSelecionada);
        Assert.AreEqual(sessao, sessaoSelecionada);
    }
}