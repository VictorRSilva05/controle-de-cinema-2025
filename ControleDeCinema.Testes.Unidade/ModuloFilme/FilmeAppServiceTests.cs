using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Aplicacao.ModuloFilme;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControleDeCinema.Testes.Unidade.ModuloFilme;

[TestClass]
[TestCategory("Testes de Unidade de Filme")]
public sealed class FilmeAppServiceTests
{
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IRepositorioFilme>? repositorioFilmeMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<FilmeAppService>>? loggerMock;

    private FilmeAppService? filmeAppService;

    [TestInitialize]
    public void Setup()
    {
        tenantProviderMock = new Mock<ITenantProvider>();
        repositorioFilmeMock = new Mock<IRepositorioFilme>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<FilmeAppService>>();
        filmeAppService = new FilmeAppService(
            tenantProviderMock.Object,
            repositorioFilmeMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object
            );
    }

    [TestMethod]
    public void Deve_Cadastrar_Filme_Com_Sucesso()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 120, false, genero);

        repositorioFilmeMock?.Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>());

        // Act
        var resultado = filmeAppService!.Cadastrar(filme);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);
    }

    [TestMethod]
    public void Deve_Retornar_Falha_Se_Titulo_Ja_Estiver_Registrado()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 120, false, genero);
        var filmeNovo = new Filme("Heat", 120, false, genero);

        repositorioFilmeMock?.Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filme });

        // Act
        var resultado = filmeAppService!.Cadastrar(filmeNovo);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filmeNovo), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Editar_Registro_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 120, false, genero);
        var filmeEditado = new Filme("Heat", 140, false, genero);

        repositorioFilmeMock?.Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filme });

        // Act
        var resultado = filmeAppService!.Editar(filme.Id, filmeEditado);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Editar(filme.Id, filmeEditado), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Falhar_Edicao_Caso_Ja_Haja_OMesmo_Titulo_Registrado()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 120, false, genero);
        var filmeEditado = new Filme("Leon The Professional", 110, false, genero);
        var filmeExistente = new Filme("Leon The Professional", 130, false, genero);

        repositorioFilmeMock?.Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filme, filmeExistente });

        // Act
        var resultado = filmeAppService!.Editar(filme.Id, filmeEditado);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Editar(filme.Id, filmeEditado), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Excluir_Filme_Com_Sucesso()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 120, false, genero);

        repositorioFilmeMock?.Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filme });

        // Act
        var resultado = filmeAppService!.Excluir(filme.Id);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Excluir(filme.Id), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);    
    }
}
