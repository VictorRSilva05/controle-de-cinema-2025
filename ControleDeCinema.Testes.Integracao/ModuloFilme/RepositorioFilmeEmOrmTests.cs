using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Integracao.Compartilhado;

namespace ControleDeCinema.Testes.Integracao.ModulogeneroFilme;

[TestClass]
[TestCategory("Testes de Integração de Filme")]
public sealed class RepositorioFilmeEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Filme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");
        repositorioGeneroFilme?.Cadastrar(generoFilme);
        dbContext?.SaveChanges();

        var filme = new Filme("The exorcist", 140, false, generoFilme);

        // Act
        repositorioFilme?.Cadastrar(filme);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioFilme?.SelecionarRegistroPorId(filme.Id);

        Assert.AreEqual(filme, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Editar_Filme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");
        var generoFilmeEditada = new GeneroFilme("ação");
        repositorioGeneroFilme?.Cadastrar(generoFilme);
        repositorioGeneroFilme?.Cadastrar(generoFilmeEditada);
        dbContext?.SaveChanges();

        var filme = new Filme("The exorcist", 140, false, generoFilme);
        repositorioFilme?.Cadastrar(filme);
        dbContext?.SaveChanges();

        var filmeEditado = new Filme("Heat", 160, false, generoFilmeEditada);

        // Act
        var conseguiuEditar = repositorioFilme?.Editar(filme.Id, filmeEditado);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioFilme?.SelecionarRegistroPorId(filme.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(filme, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_Filme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");
        repositorioGeneroFilme?.Cadastrar(generoFilme);
        dbContext?.SaveChanges();

        var filme = new Filme("The exorcist", 140, false, generoFilme);
        repositorioFilme?.Cadastrar(filme);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = repositorioFilme?.Excluir(filme.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioFilme?.SelecionarRegistroPorId(filme.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Filmes_Corretamente()
    {

        // Arrange
        var generoFilme1 = new GeneroFilme("medo");
        var generoFilme2 = new GeneroFilme("ação");
        var generoFilme3 = new GeneroFilme("suspense");

        List<GeneroFilme> generoFilmes = [generoFilme1, generoFilme2, generoFilme3];

        repositorioGeneroFilme?.CadastrarEntidades(generoFilmes);
        dbContext?.SaveChanges();

        var filme1 = new Filme("The exorcist", 140, false, generoFilme1);
        var filme2 = new Filme("Heat", 160, false, generoFilme2);
        var filme3 = new Filme("Trainspotting", 150, false, generoFilme3);

        List<Filme> filmesEsperados = [filme1, filme2, filme3];

        repositorioFilme?.CadastrarEntidades(filmesEsperados);
        dbContext?.SaveChanges();

        var filmesEsperadosOrdenadas = filmesEsperados
            .OrderBy(s => s.Titulo)
            .ToList();

        // Act
        var filmesRecebidos = repositorioFilme?
            .SelecionarRegistros()
            .OrderBy(s => s.Titulo)
            .ToList();

        // Assert
        CollectionAssert.AreEqual(filmesEsperadosOrdenadas, filmesRecebidos);
    }
}
