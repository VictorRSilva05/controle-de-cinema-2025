using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Aplicacao.Compartilhado;
using ControleDeCinema.Aplicacao.ModuloFilme;
using ControleDeCinema.Aplicacao.ModuloSessao;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;
using Microsoft.Extensions.Logging;
using Moq;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace ControleDeCinema.Testes.Unidade.ModuloSessao;

[TestClass]
[TestCategory("Testes de Unidade de Sessao")]
public sealed class SessaoAppServiceTests
{
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IRepositorioSessao>? repositorioSessaoMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<SessaoAppService>>? loggerMock;

    private SessaoAppService? sessaoAppService;

    [TestInitialize]
    public void Setup()
    {
        tenantProviderMock = new Mock<ITenantProvider>();
        repositorioSessaoMock = new Mock<IRepositorioSessao>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<SessaoAppService>>();
        sessaoAppService = new SessaoAppService(
            tenantProviderMock.Object,
            repositorioSessaoMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object
            );
    }

    [TestMethod]
    public void Deve_Cadastrar_Sessao_Com_Sucesso()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 160, false, genero);
        var sala = new Sala(1, 100);
        var sessao = new Sessao(DateTime.Parse("1/11/2025 20:00:00"), 100, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { });

        // Act
        var resultado = sessaoAppService!.Cadastrar(sessao);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Retornar_Falha_Caso_ONumero_De_Ingressos_Exceda_O_Limite_Da_Sala()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 160, false, genero);
        var sala = new Sala(1, 90);
        var sessao = new Sessao(DateTime.Parse("1/11/2025 20:00:00"), 100, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { });

        // Act
        var resultado = sessaoAppService!.Cadastrar(sessao);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Retornar_Falha_Caso_Ja_Exista_Uma_Sessao_Neste_Horario()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 160, false, genero);
        var sala = new Sala(1, 100);
        var sessao = new Sessao(DateTime.Parse("1/11/2025 20:00:00"), 100, filme, sala);

        var filme2 = new Filme("Leon the Professional", 160, false, genero);
        var sessao2 = new Sessao(DateTime.Parse("1/11/2025 20:00:00"), 100, filme2, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessao });

        // Act
        var resultado = sessaoAppService!.Cadastrar(sessao2);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao2), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Editar_Sessao_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 160, false, genero);
        var sala = new Sala(1, 100);
        var sessao = new Sessao(DateTime.Parse("1/11/2025 20:00:00"), 100, filme, sala);

        var filme2 = new Filme("Leon the Professional", 160, false, genero);
        var sessaoEditada = new Sessao(DateTime.Parse("1/11/2025 19:00:00"), 100, filme2, sala);

        repositorioSessaoMock?
          .Setup(r => r.SelecionarRegistros())
          .Returns(new List<Sessao>() { sessao });

        repositorioSessaoMock?
            .Setup(r => r.Editar(sessao.Id, sessaoEditada))
            .Returns(true);

        // Act
        var resultado = sessaoAppService!.Editar(sessao.Id, sessaoEditada);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Editar(sessao.Id, sessaoEditada), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Falhar_Edicao_Caso_ONumero_De_Ingressos_Exceda_OLimite()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 160, false, genero);
        var sala = new Sala(1, 100);
        var sessao = new Sessao(DateTime.Parse("1/11/2025 20:00:00"), 100, filme, sala);

        var sessaoEditada = new Sessao(DateTime.Parse("1/11/2025 21:00:00"), 110, filme, sala);

        repositorioSessaoMock?
         .Setup(r => r.SelecionarRegistros())
         .Returns(new List<Sessao>() { sessao });

        repositorioSessaoMock?
            .Setup(r => r.Editar(sessao.Id, sessaoEditada))
            .Returns(false);

        // Act
        var resultado = sessaoAppService!.Editar(sessao.Id, sessaoEditada);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Editar(sessao.Id, sessaoEditada), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Falhar_Edicao_Caso_Ja_Exista_Uma_Sessao_Neste_Horario()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 160, false, genero);
        var sala = new Sala(1, 100);
        var sessao = new Sessao(DateTime.Parse("1/11/2025 20:00:00"), 100, filme, sala);

        var filme2 = new Filme("Leon the Professional", 160, false, genero);
        var sessao2 = new Sessao(DateTime.Parse("1/11/2025 23:00:00"), 100, filme2, sala);

        var sessaoEditada = new Sessao(DateTime.Parse("1/11/2025 20:00:00"), 100, filme2, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessao, sessao2 });

        repositorioSessaoMock?
            .Setup(r => r.Editar(sessao2.Id, sessaoEditada))
            .Returns(false);

        // Act
        var resultado = sessaoAppService!.Editar(sessao2.Id, sessaoEditada);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Editar(sessao2.Id, sessaoEditada), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);


        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Excluir_Sessao_Com_Sucesso()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Heat", 160, false, genero);
        var sala = new Sala(1, 100);
        var sessao = new Sessao(DateTime.Parse("1/11/2025 20:00:00"), 100, filme, sala);

        repositorioSessaoMock?
             .Setup(r => r.SelecionarRegistros())
             .Returns(new List<Sessao>() { sessao });

        repositorioSessaoMock?
            .Setup(r => r.Excluir(sessao.Id))
            .Returns(true);

        // Act
        var resultado = sessaoAppService!.Excluir(sessao.Id);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Excluir(sessao.Id), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }
}
