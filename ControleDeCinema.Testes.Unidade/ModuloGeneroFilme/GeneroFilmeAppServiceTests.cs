using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Aplicacao.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControleDeCinema.Testes.Unidade.ModuloGeneroFilme;

[TestClass]
[TestCategory("Testes de Unidade de GeneroFilme")]
public class GeneroFilmeAppServiceTests
{
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IRepositorioGeneroFilme>? repositorioGeneroFilmeMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<GeneroFilmeAppService>>? loggerMock;

    private GeneroFilmeAppService? generoFilmeAppService;

    [TestInitialize]
    public void Setup()
    {
        tenantProviderMock = new Mock<ITenantProvider>();
        repositorioGeneroFilmeMock = new Mock<IRepositorioGeneroFilme>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<GeneroFilmeAppService>>();
        generoFilmeAppService = new GeneroFilmeAppService(
            tenantProviderMock.Object,
            repositorioGeneroFilmeMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object
            );
    }

    [TestMethod]
    public void Deve_Cadastrar_GeneroFilme_Com_Sucesso()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>());

        // Act
        var resultado = generoFilmeAppService!.Cadastrar(generoFilme);

        // Assert
        Assert.IsTrue(resultado.IsSuccess); 
        repositorioGeneroFilmeMock?.Verify(r => r.Cadastrar(generoFilme), Times.Once());
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once());
    }

    [TestMethod]
    public void Deve_Retornar_Falha_Caso_Descricao_Ja_Esteja_Registrada()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");
        var generoFilmeNovo = new GeneroFilme("medo");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>() { generoFilme });

        // Act
        var resultado = generoFilmeAppService!.Cadastrar(generoFilmeNovo);

        // Assert
        Assert.IsFalse(resultado.IsSuccess);
        repositorioGeneroFilmeMock?.Verify(r => r.Cadastrar(generoFilme), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);
    }

    [TestMethod]
    public void Deve_Editar_Registro_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");
        var generoFilmeEditado = new GeneroFilme("medonho");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme> { generoFilme });

        // Act
        var resultado = generoFilmeAppService!.Editar(generoFilme.Id, generoFilmeEditado);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioGeneroFilmeMock?.Verify(r => r.Editar(generoFilme.Id, generoFilmeEditado), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);
    }

    [TestMethod]
    public void Deve_Falhar_Edicao_Caso_Ja_Hajo_OMesmo_Genero_Registrado()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");
        var generoFilmeEditado = new GeneroFilme("odem");
        var generoFilmeExistente = new GeneroFilme("odem");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>() {generoFilme, generoFilmeExistente });

        // Act
        var resultado = generoFilmeAppService!.Editar(generoFilme.Id, generoFilmeEditado);

        //Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Editar(generoFilme.Id, generoFilmeEditado), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Excluir_GeneroFilme_Com_Sucesso()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme> { generoFilme });

        // Act
        var resultado = generoFilmeAppService!.Excluir(generoFilme.Id);

        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Excluir(generoFilme.Id), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }
}
