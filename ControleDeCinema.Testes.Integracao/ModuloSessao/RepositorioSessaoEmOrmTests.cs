using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Testes.Integracao.Compartilhado;

namespace ControleDeCinema.Testes.Integracao.ModulogeneroFilme;

[TestClass]
[TestCategory("Testes de Integração de Sessao")]
public sealed class RepositorioSessaoEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Sessao_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");
        repositorioGeneroFilme?.Cadastrar(generoFilme);
        dbContext?.SaveChanges();

        var filme = new Filme("The exorcist", 140, false, generoFilme);
        repositorioFilme?.Cadastrar(filme);
        dbContext?.SaveChanges();

        var sala = new Sala(1, 100);
        repositorioSala?.Cadastrar(sala);
        dbContext?.SaveChanges();

        var sessao = new Sessao(DateTime.SpecifyKind(DateTime.Parse("1/11/2025 8:00:00 PM"), DateTimeKind.Utc), 100, filme, sala);

        // Act
        repositorioSessao?.Cadastrar(sessao);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioSessao?.SelecionarRegistroPorId(sessao.Id);

        Assert.AreEqual(sessao, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Editar_Sessao_Corretamente()
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
        repositorioFilme?.Cadastrar(filmeEditado);
        dbContext?.SaveChanges();

        var sala = new Sala(1, 100);
        repositorioSala?.Cadastrar(sala);
        dbContext?.SaveChanges();

        var sessao = new Sessao(DateTime.SpecifyKind(DateTime.Parse("1/11/2025 8:00:00 PM"), DateTimeKind.Utc), 100, filme, sala);
        repositorioSessao?.Cadastrar(sessao);
        dbContext?.SaveChanges();

        var sessaoEditada = new Sessao(DateTime.SpecifyKind(DateTime.Parse("1/11/2025 9:00:00 PM"), DateTimeKind.Utc), 100, filmeEditado, sala);

        // Act
        var conseguiuEditar = repositorioSessao?.Editar(sessao.Id, sessaoEditada);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioSessao?.SelecionarRegistroPorId(sessao.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(sessao, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_Sessao_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("medo");
        repositorioGeneroFilme?.Cadastrar(generoFilme);
        dbContext?.SaveChanges();

        var filme = new Filme("The exorcist", 140, false, generoFilme);
        repositorioFilme?.Cadastrar(filme);
        dbContext?.SaveChanges();

        var sala = new Sala(1, 100);
        repositorioSala?.Cadastrar(sala);
        dbContext?.SaveChanges();

        var sessao = new Sessao(DateTime.SpecifyKind(DateTime.Parse("1/11/2025 8:00:00 PM"), DateTimeKind.Utc), 100, filme, sala);
        repositorioSessao?.Cadastrar(sessao);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = repositorioSessao?.Excluir(sessao.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioSessao?.SelecionarRegistroPorId(sessao.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Sessoes_Corretamente()
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

        List<Filme> filmes = [filme1, filme2, filme3];

        repositorioFilme?.CadastrarEntidades(filmes);
        dbContext?.SaveChanges();

        var sala1 = new Sala(1, 100);
        var sala2 = new Sala(2, 100);
        var sala3 = new Sala(3, 100);

        List<Sala> salas = [sala1, sala2, sala3];

        repositorioSala?.CadastrarEntidades(salas);
        dbContext?.SaveChanges();

        var sessao1 = new Sessao(DateTime.SpecifyKind(DateTime.Parse("1/11/2025 8:00:00 PM"), DateTimeKind.Utc),100,filme1, sala1);
        var sessao2 = new Sessao(DateTime.SpecifyKind(DateTime.Parse("1/11/2025 11:00:00 PM"), DateTimeKind.Utc),100,filme2, sala2);
        var sessao3 = new Sessao(DateTime.SpecifyKind(DateTime.Parse("2/11/2025 3:00:00 AM"), DateTimeKind.Utc),100,filme3, sala3);

        List<Sessao> sessoesEsperadas = [sessao1, sessao2, sessao3];

        repositorioSessao?.CadastrarEntidades(sessoesEsperadas);
        dbContext?.SaveChanges();

        var sessoesEsperadasOrdenadas = sessoesEsperadas
            .OrderBy(s => s.Filme.Titulo)
            .ToList();

        // Act
        var sessoesRecebidas = repositorioSessao?
            .SelecionarRegistros()
            .OrderBy(s => s.Filme.Titulo)
            .ToList();

        // Assert
        CollectionAssert.AreEqual(sessoesEsperadasOrdenadas, sessoesRecebidas);
    }
}
