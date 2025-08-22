using Castle.Core.Logging;
using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Aplicacao.ModuloSala;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloSala;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControleDeCinema.Testes.Unidade.ModuloSala;

[TestClass]
[TestCategory("Testes de Unidade de Disciplina")]
public sealed class SalaAppServiceTests
{
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IRepositorioSala>? repositorioSalaMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<SalaAppService>>? loggerMock;

    private SalaAppService? salaAppService;

    [TestInitialize]
    public void Setup()
    {
        tenantProviderMock = new Mock<ITenantProvider>();
        repositorioSalaMock = new Mock<IRepositorioSala>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<SalaAppService>>();
        salaAppService = new SalaAppService(
            tenantProviderMock.Object,
            repositorioSalaMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object
        );
    }

    [TestMethod]
    public void Deve_Cadastrar_Sala_Com_Sucesso()
    {
        // Arrange
        var sala = new Sala(1, 100);

        repositorioSalaMock?.Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>());

        // Act
        var resultado = salaAppService!.Cadastrar(sala);

        // Assert
        Assert.IsTrue(resultado.IsSuccess);
        repositorioSalaMock?.Verify(r => r.Cadastrar(sala), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);
    }

    [TestMethod]
    public void Deve_Retornar_Falha_Se_Numero_Ja_Estiver_Registrado()
    {
        // Arrange
        var sala = new Sala(1, 100);

        var salaTeste = new Sala(1, 100);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>() { salaTeste });

        // Act
        var resultado = salaAppService!.Cadastrar(sala);

        // Assert
        repositorioSalaMock?.Verify(r => r.Cadastrar(sala), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Deve_Editar_Registro_Corretamente()
    {
        // Arrange
        var sala = new Sala(1, 100);

        var salaEditada = new Sala(2, 50);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>() { sala });

        // Act 
        var resultado = salaAppService!.Editar(sala.Id, salaEditada);

        // Assert
        repositorioSalaMock?.Verify(r => r.Editar(sala.Id, salaEditada), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Falhar_Edicao_Caso_Ja_Haja_OMesmo_Numero_Registrado()
    {
        // Arrange
        var sala = new Sala(1, 100);
        var salaEditada = new Sala(3, 50);
        var salaExistente = new Sala(3, 50);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala> { sala, salaExistente });

        // Act
        var resultado = salaAppService!.Editar(sala.Id, salaEditada);

        // Assert
        repositorioSalaMock?.Verify(r => r.Editar(sala.Id, salaEditada), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Excluir_Sala_Com_Sucesso()
    {
        // Arrange
        var sala = new Sala(1, 100);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala> { sala });
        
        // Act
        var resultado = salaAppService!.Excluir(sala.Id);

        //Assert
        repositorioSalaMock?.Verify(r => r.Excluir(sala.Id), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }
}
