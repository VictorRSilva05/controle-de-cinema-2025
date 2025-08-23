using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Integracao.Compartilhado;

namespace ControleDeCinema.Testes.Integracao.ModulogeneroFilme;

[TestClass]
[TestCategory("Testes de Integração de GeneroFilme")]
public sealed class RepositorioGeneroFilmeEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");

        // Act
        repositorioGeneroFilme?.Cadastrar(generoFilme);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioGeneroFilme?.SelecionarRegistroPorId(generoFilme.Id);

        Assert.AreEqual(generoFilme, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Editar_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");
        var generoFilmeEditada = new GeneroFilme("ação");
        repositorioGeneroFilme?.Cadastrar(generoFilme);
        dbContext?.SaveChanges();

        // Act
        var conseguiuEditar = repositorioGeneroFilme?.Editar(generoFilme.Id, generoFilmeEditada);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioGeneroFilme?.SelecionarRegistroPorId(generoFilme.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(generoFilme, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");
        repositorioGeneroFilme?.Cadastrar(generoFilme);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = repositorioGeneroFilme?.Excluir(generoFilme.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioGeneroFilme?.SelecionarRegistroPorId(generoFilme.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_generoFilmes_Corretamente()
    {

        // Arrange
        var generoFilme1 = new GeneroFilme("medo");
        var generoFilme2 = new GeneroFilme("ação");
        var generoFilme3 = new GeneroFilme("suspense");

        List<GeneroFilme> generoFilmesEsperadas = [generoFilme1, generoFilme2, generoFilme3];

        repositorioGeneroFilme?.CadastrarEntidades(generoFilmesEsperadas);
        dbContext?.SaveChanges();

        var generoFilmesEsperadasOrdenadas = generoFilmesEsperadas
            .OrderBy(s => s.Descricao)
            .ToList();

        // Act
        var generoFilmesRecebidas = repositorioGeneroFilme?
            .SelecionarRegistros()
            .OrderBy(s => s.Descricao)
            .ToList();

        // Assert
        CollectionAssert.AreEqual(generoFilmesEsperadasOrdenadas, generoFilmesRecebidas);
    }
}
