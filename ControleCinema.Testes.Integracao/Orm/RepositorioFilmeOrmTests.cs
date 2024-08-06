using ControleCinema.Dominio.ModuloFilme;
using ControleCinema.Dominio.ModuloGenero;
using ControleCinema.Dominio.ModuloSala;
using ControleCinema.Dominio.ModuloSessao;
using ControleCinema.Infra.Orm.Compartilhado;
using ControleCinema.Infra.Orm.ModuloFilme;
using ControleCinema.Infra.Orm.ModuloGenero;
using ControleCinema.Infra.Orm.ModuloSala;
using ControleCinema.Infra.Orm.ModuloSessao;

namespace ControleCinema.Testes.Integracao.Orm
{
    [TestClass]
    public class RepositorioFilmeOrmTests
    {
        private readonly ControleCinemaDbContext db;

        public RepositorioFilmeOrmTests()
        {
            db = new ControleCinemaDbContext();

            db.Set<Sessao>().RemoveRange(db.Set<Sessao>());
            db.Set<Sala>().RemoveRange(db.Set<Sala>());
            db.Set<Filme>().RemoveRange(db.Set<Filme>());
            db.Set<Genero>().RemoveRange(db.Set<Genero>());

            db.SaveChanges();
        }

        [TestMethod]
        public void Deve_Inserir_Filme()
        {
            //arrange
            Genero genero = new Genero("Ação");
            var repositorioGenero = new RepositorioGeneroEmOrm(db);
            repositorioGenero.Inserir(genero);


            Filme novoFilme = new Filme("Rambo", 2, genero, true);
            var repositorio = new RepositorioFilmeEmOrm(db);

            //action
            repositorio.Inserir(novoFilme);
            db.SaveChanges();

            //assert
            Filme filmeEncontrado = repositorio.SelecionarPorId(novoFilme.Id);

            Assert.IsNotNull(filmeEncontrado);
            Assert.AreEqual(novoFilme.Id, filmeEncontrado.Id);
            Assert.AreEqual(novoFilme.Titulo, filmeEncontrado.Titulo);
            Assert.AreEqual(novoFilme.Duracao, filmeEncontrado.Duracao);
            Assert.AreEqual(novoFilme.Lancamento, filmeEncontrado.Lancamento);
            Assert.AreEqual(novoFilme.Genero, filmeEncontrado.Genero);

            Assert.AreEqual(0, novoFilme.Sessoes.Count);

        }

        [TestMethod]
        public void Deve_Editar_Filme()
        {
            //arrange
            var repositorioGenero = new RepositorioGeneroEmOrm(db);

            Genero generoAcao = new Genero("Ação");
            Genero generoSuspense = new Genero("Suspense");

            repositorioGenero.Inserir(generoAcao);
            repositorioGenero.Inserir(generoSuspense);

            Filme novoFilme = new Filme("Rambo", 2, generoAcao, true);
            var repositorio = new RepositorioFilmeEmOrm(db);
            repositorio.Inserir(novoFilme);

            //action
            Filme? filmeAtualizado = repositorio.SelecionarPorId(novoFilme.Id);
            filmeAtualizado.Titulo = "Predador";
            filmeAtualizado.Duracao = 3;
            filmeAtualizado.Genero = generoSuspense;
            filmeAtualizado.Lancamento = false;

            repositorio.Editar(novoFilme);
            db.SaveChanges();

            //assert
            Filme? filmeEncontrado = repositorio.SelecionarPorId(novoFilme.Id);

            Assert.IsNotNull(filmeEncontrado);
            Assert.AreEqual(filmeAtualizado.Id, filmeEncontrado.Id);
            Assert.AreEqual(filmeAtualizado.Titulo, filmeEncontrado.Titulo);
            Assert.AreEqual(filmeAtualizado.Duracao, filmeEncontrado.Duracao);
            Assert.AreEqual(filmeAtualizado.Lancamento, filmeEncontrado.Lancamento);
            Assert.AreEqual(filmeAtualizado.Genero, filmeEncontrado.Genero);

            Assert.AreEqual(0, novoFilme.Sessoes.Count);

        }

        [TestMethod]
        public void Deve_Excluir_Filme()
        {
            //arrange
            var repositorioGenero = new RepositorioGeneroEmOrm(db);

            Genero generoAcao = new Genero("Ação");

            repositorioGenero.Inserir(generoAcao);

            Filme filme = new Filme("Rambo", 2, generoAcao, true);
            var repositorio = new RepositorioFilmeEmOrm(db);
            repositorio.Inserir(filme);

            //action
            repositorio.Excluir(filme);
            db.SaveChanges();

            //assert
            Filme? filmeEncontrado = repositorio.SelecionarPorId(filme.Id);

            Assert.IsNull(filmeEncontrado);
        }

        [TestMethod]
        public void Deve_Selecionar_Filme_Com_Sessoes_Por_Id()
        {
            //arrange
            Sala sala = new Sala(12, 20);
            Genero generoAcao = new Genero("Ação");

            var repositorioGenero = new RepositorioGeneroEmOrm(db);
            var repositorioSala = new RepositorioSalaEmOrm(db);
            var repositorioSessao = new RepositorioSessaoEmOrm(db);

            repositorioGenero.Inserir(generoAcao);
            repositorioSala.Inserir(sala);

            Filme novoFilme = new Filme("Rambo", 2, generoAcao, true);
            var repositorio = new RepositorioFilmeEmOrm(db);
            repositorio.Inserir(novoFilme);

            Sessao sessao1 = new Sessao(novoFilme, sala, 20, DateTime.Now);
            Sessao sessao2 = new Sessao(novoFilme, sala, 20, DateTime.Now.AddHours(4));

            repositorioSessao.Inserir(sessao1);
            repositorioSessao.Inserir(sessao2);

            repositorio = new RepositorioFilmeEmOrm(new ControleCinemaDbContext());
            Filme? filmeEncontrado = repositorio.SelecionarPorId(novoFilme.Id);

            Assert.IsNotNull(filmeEncontrado);

            Assert.AreEqual(filmeEncontrado.Genero, filmeEncontrado.Genero);
            Assert.AreEqual(2, novoFilme.Sessoes.Count);
        }

        [TestMethod]
        public void Deve_Selecionar_Varios_Filmes()
        {
            //arrange
            Genero generoAcao = new Genero("Ação");

            new RepositorioGeneroEmOrm(db).Inserir(generoAcao);
            
            var repositorio = new RepositorioFilmeEmOrm(db);

            Filme novoFilme1 = new Filme("Rambo 1", 2, generoAcao, true);
            repositorio.Inserir(novoFilme1);

            Filme novoFilme2 = new Filme("Rambo 2", 2, generoAcao, true);
            repositorio.Inserir(novoFilme2);

            repositorio = new RepositorioFilmeEmOrm(new ControleCinemaDbContext());
            List<Filme> filmes = repositorio.SelecionarTodos();

            Assert.IsNotNull(filmes);

            Assert.AreEqual(2, filmes.Count);
        }
    }
}