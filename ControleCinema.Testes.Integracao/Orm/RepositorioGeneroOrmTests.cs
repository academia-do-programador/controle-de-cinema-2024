using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Infra.Orm.Compartilhado;
using ControleCinema.Infra.Orm.ModuloGenero;

namespace ControleCinema.Testes.Integracao.Orm;

[TestClass]
public class RepositorioGeneroOrmTests
{
    private ControleCinemaDbContext db;
    private RepositorioGeneroEmOrm repositorioGenero;

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

        repositorioGenero = new RepositorioGeneroEmOrm(db);
    }

    [TestMethod]
    public void Deve_Inserir_Genero()
    {
        // Arrange
        var novoGenero = new Genero("Ação");

        // Act
        repositorioGenero.Inserir(novoGenero);

        // Assert
        var generoEncontrado = repositorioGenero.SelecionarPorId(novoGenero.Id);

        Assert.IsNotNull(generoEncontrado);
        Assert.AreEqual(novoGenero.Id, generoEncontrado.Id);
        Assert.AreEqual(novoGenero.Descricao, generoEncontrado.Descricao);
    }


    [TestMethod]
    public void Deve_Editar_Genero()
    {
        // Arrange
        var novoGenero = new Genero("Ação");
        repositorioGenero.Inserir(novoGenero);

        novoGenero.Descricao = "Aventura";
        
        // Act
        repositorioGenero.Editar(novoGenero);        

        // Assert
        var generoEncontrado = repositorioGenero.SelecionarPorId(novoGenero.Id);

        Assert.IsNotNull(generoEncontrado);
        Assert.AreEqual(novoGenero, generoEncontrado);
    }

    [TestMethod]
    public void Deve_Excluir_Genero()
    {
        // Arrange
        var novoGenero = new Genero("Ação");
        repositorioGenero.Inserir(novoGenero);
        
        // Act
        repositorioGenero.Excluir(novoGenero);
        
        // Assert
        var generoEncontrado = repositorioGenero.SelecionarPorId(novoGenero.Id);

        Assert.IsNull(generoEncontrado);
    }

    [TestMethod]
    public void Deve_Selecionar_Generos()
    {
        Genero[] generosParaInserir =
        [
            new Genero("Ação"),
            new Genero("Aventura"),
            new Genero("Terror")
        ];
        
        foreach (var genero in generosParaInserir)
            repositorioGenero.Inserir(genero);

        var generosSelecionados = repositorioGenero.SelecionarTodos();
        
        CollectionAssert.AreEqual(generosParaInserir, generosSelecionados);
    }
}