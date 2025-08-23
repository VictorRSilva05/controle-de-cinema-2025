using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Testes.Integracao.Compartilhado;

namespace ControleDeCinema.Testes.Integracao.ModuloSala;

[TestClass]
[TestCategory("Testes de Integração de Sala")]
public sealed class RepositorioSalaEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Sala_Corretamente()
    {
        // Arrange
        var sala = new Sala(1, 100);

        // Act
        repositorioSala?.Cadastrar(sala);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioSala?.SelecionarRegistroPorId(sala.Id);

        Assert.AreEqual(sala, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Editar_Sala_Corretamente()
    {
        // Arrange
        var sala = new Sala(1, 100);
        var salaEditada = new Sala(2, 75);
        repositorioSala?.Cadastrar(sala);
        dbContext?.SaveChanges();

        // Act
        var conseguiuEditar = repositorioSala?.Editar(sala.Id, salaEditada);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioSala?.SelecionarRegistroPorId(sala.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(sala, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_Sala_Corretamente()
    {
        // Arrange
        var sala = new Sala(1, 100);
        repositorioSala?.Cadastrar(sala);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = repositorioSala?.Excluir(sala.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioSala?.SelecionarRegistroPorId(sala.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Salas_Corretamente()
    {
        // Arrange
        var sala1 = new Sala(1, 100);
        var sala2 = new Sala(2, 100);
        var sala3 = new Sala(3, 100);

        List<Sala> salasEsperadas = [sala1, sala2, sala3];

        repositorioSala?.CadastrarEntidades(salasEsperadas);
        dbContext?.SaveChanges();

        var salasEsperadasOrdenadas = salasEsperadas
            .OrderBy(s => s.Numero)
            .ToList();

        // Act
        var salasRecebidas = repositorioSala?
            .SelecionarRegistros()
            .OrderBy(s => s.Numero)
            .ToList();

        // Assert
        CollectionAssert.AreEqual(salasEsperadasOrdenadas, salasRecebidas);
    }
}
