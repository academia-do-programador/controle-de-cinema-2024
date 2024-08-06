using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Infra.Orm.Compartilhado;
using ControleCinema.Infra.Orm.ModuloSala;

namespace ControleCinema.Testes.Integracao.Orm;

public class RepositorioSalaOrmTests
{
    private ControleCinemaDbContext db;
    private RepositorioSalaEmOrm repositorioSala;

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

        repositorioSala = new RepositorioSalaEmOrm(db);
    }

    [TestMethod]
    public void Deve_Inserir_Sala()
    {
        var novaSala = new Sala(1, 30);
        
        repositorioSala.Inserir(novaSala);

        var salaSelecionada = repositorioSala.SelecionarPorId(novaSala.Id);
        
        Assert.IsNotNull(salaSelecionada);
        Assert.AreEqual(novaSala, salaSelecionada);
    }
    
    [TestMethod]
    public void Deve_Editar_Sala()
    {
        var novaSala = new Sala(1, 30);
        
        repositorioSala.Inserir(novaSala);

        novaSala.Numero = 5;
        
        repositorioSala.Editar(novaSala);
        
        var salaSelecionada = repositorioSala.SelecionarPorId(novaSala.Id);
        
        Assert.IsNotNull(salaSelecionada);
        Assert.AreEqual(novaSala, salaSelecionada);
    }
    
    [TestMethod]
    public void Deve_Excluir_Sala()
    {
        var novaSala = new Sala(1, 30);
        
        repositorioSala.Inserir(novaSala);
        
        repositorioSala.Excluir(novaSala);
        
        var salaSelecionada = repositorioSala.SelecionarPorId(novaSala.Id);
        
        Assert.IsNull(salaSelecionada);
    }
    
    [TestMethod]
    public void Deve_Selecionar_Sala()
    {
        Sala[] salasParaInserir =
        [
            new Sala(1, 30),
            new Sala(5, 25),
            new Sala(6, 28)
        ];
        
        foreach (var sala in salasParaInserir)
            repositorioSala.Inserir(sala);

        var salasSelecionadas = repositorioSala.SelecionarTodos();
        
        CollectionAssert.AreEqual(salasParaInserir, salasSelecionadas);
    }
}